using Device.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#if (!LIBUSB)
using Usb.Net.Windows;
using Hid.Net.Windows;
//using SerialPort.Net.Windows;
#else
using Device.Net.LibUsb;
#endif

namespace LDToypad
{
    /// <summary>
    /// Event args for internal events
    /// </summary>
    internal class LDDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// the raw response from the device
        /// </summary>
        public byte[] Response { get; private set; }

        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        /// <param name="response">The raw response from the device</param>
        public LDDeviceEventArgs(byte[] response) : base()
        {
            this.Response = response;
        }
    }

    internal class Device : IDisposable
    {
        #region Fields
#if (LIBUSB)
        private const int PollMilliseconds = 6000;
#else
        private const int PollMilliseconds = 3000;
#endif
        //Define the types of devices to search for. This particular device can be connected to via USB, or Hid
        private readonly List<FilterDeviceDefinition> _DeviceDefinitions = new List<FilterDeviceDefinition>
        {
            new FilterDeviceDefinition{ DeviceType= DeviceType.Hid, VendorId=0x0e6f, ProductId=0x0241, Label="LEGO READER V2.10" }
        };
        #endregion

        #region Events
        public event EventHandler DeviceInitialized;
        public event EventHandler DeviceDisconnected;

        public event EventHandler<LDDeviceEventArgs> DataRecieved;
        #endregion

        #region Public Properties
        public IDevice ToypadDevice { get; private set; }
        public DeviceListener DeviceListener { get; }
        #endregion

        private CancellationTokenSource cts = null;
        #region Constructor
        public Device()
        {            //Register the factory for creating Usb devices. This only needs to be done once.
#if (LIBUSB)
            LibUsbUsbDeviceFactory.Register(Logger, Tracer);
#else
            WindowsUsbDeviceFactory.Register(Logger, Tracer);
            WindowsHidDeviceFactory.Register(Logger, Tracer);
            //WindowsSerialPortDeviceFactory.Register(Logger, Tracer);
#endif
            DeviceListener = new DeviceListener(_DeviceDefinitions, PollMilliseconds) { Logger = new DebugLogger() };
            DeviceListener.DeviceInitialized += DevicePoller_DeviceInitialized;
            DeviceListener.DeviceDisconnected += DevicePoller_DeviceDisconnected;
        }
        #endregion

        #region Event Handlers
        private void DevicePoller_DeviceInitialized(object sender, DeviceEventArgs e)
        {
            ToypadDevice = e.Device;
            DeviceInitialized?.Invoke(this, new EventArgs());
        }

        private void DevicePoller_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            ToypadDevice = null;
            DeviceDisconnected?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            if (cts == null)
            {
                cts = new CancellationTokenSource();
            }
            CancellationToken cancellationToken = cts.Token;

            Task pollTask = Task.Factory.StartNew(
                async () => {
                    await MainLoopAsync(cancellationToken);
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );
        }

        public void Stop()
        {
            cts?.Cancel();
        }

        private async Task MainLoopAsync(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    // Clean up here, then...
                    token.ThrowIfCancellationRequested();
                }

                byte[] result = await ToypadDevice.ReadAsync();
                if(result != null && result.Length > 0)
                {
                    this.DataRecieved?.Invoke(this, new LDDeviceEventArgs(result));
                }

                await Task.Delay(250);
            }
        }

        //! Keep stuff below this line
        private static readonly DebugLogger Logger = new DebugLogger();
        private static readonly DebugTracer Tracer = new DebugTracer();

        public async Task Connect()
        {
            Console.WriteLine("Connecting to device");
            // close any existing device
            this.Disconnect();

            // Get the first available device and connect to it
            var devices = await DeviceManager.Current.GetDevicesAsync(_DeviceDefinitions);
            ToypadDevice = devices.FirstOrDefault();

            // No devices? Throw an exception
            if (ToypadDevice == null) throw new Exception("There were no devices found");

            // initilize the device
            await ToypadDevice.InitializeAsync();

            // wait a little just to make sure the device *is* actually initialized...
            await Task.Delay(250);

            // now send the handshake
            Console.WriteLine("Sending handshake");
            Task task = ToypadDevice.WriteAsync(new byte[] {
                0x00, 0x55, 0x0f, 0xb0, 0x01, 0x28, 0x63, 0x29, 0x20, 0x4c, 0x45, 0x47, 0x4f, 0x20, 0x32, 0x30, 0x31,
                0x34, 0xf7, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            });

            if (await Task.WhenAny(task, Task.Delay(1000)) == task)
            {
                
            }
            else
            {
                throw new TimeoutException();
            }
        }

        private byte _counter = 0;

        public async Task<int> SendPacket(byte[] command, byte[] data)
        {
            int counter = (this._counter++ & 0xFF);
            byte checksum = (byte)((command.Sum(x => x) + data.Sum(x => x) + counter) & 0xFF);
            int padAmount = 30 - command.Length - data.Length;

            byte[] packet = new byte[1]
                .Concat(command)
                .Append((byte)counter) //.Append((byte)0)
                .Concat(data)
                .Append(checksum)
                .Concat(new byte[padAmount])
                .ToArray();

            Console.WriteLine("[Writing] " + string.Join(' ', packet.Select(x => x.ToString("X2"))));

            Task task = ToypadDevice.WriteAsync(packet);
            if (await Task.WhenAny(task, Task.Delay(1000)) == task)
            {
                return counter;
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public void Disconnect()
        {
            ToypadDevice?.Close();
            // wait a bit for it to actually close
            Task.Delay(500).Wait();
            ToypadDevice?.Dispose();
            ToypadDevice = null;
        }

        public void Dispose()
        {
            if(this.ToypadDevice != null) this.Disconnect();
            DeviceListener.DeviceDisconnected -= DevicePoller_DeviceDisconnected;
            DeviceListener.DeviceInitialized -= DevicePoller_DeviceInitialized;
            DeviceListener.Dispose();
        }
        #endregion

    }
}
