using Device.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LDToypad
{
    /// <summary>
    /// A class to interface with the toypad, based on https://github.com/jonyo/toypad
    /// </summary>
    public class Toypad : IDisposable
    {
        /// <summary>
        /// Gets or sets the device.
        private Device Device { get; set; }

        public PanelGroup Panels { get; set; }

        /// <summary>
        /// Gets or sets the known minifigs by uid.
        /// </summary>
        private Dictionary<string, Tag> KnownTags { get; set; }

        /// <summary>
        /// Gets or sets the minifig names for translating from Uid to human-readable name.
        /// This must be provided by the caller. Any unknown names will just stay as null.
        /// </summary>
        public Dictionary<string, string> TagNames { get; set; }

        /// <summary>
        /// Gets or sets the pending colour changes.
        /// </summary>
        private Queue<PanelPosition> PendingColourChanges { get; set; }

        /// <summary>
        /// Gets or sets the pending minifig data.
        /// </summary>
        private Dictionary<int, TagScanEventArgs> PendingTagData { get; set; }

        /// <summary>
        /// Occurs when a minifig is scanned.
        /// </summary>
        public event EventHandler<TagScanEventArgs> TagScanned;

        /// <summary>
        /// Occurs when a minifig is added to the toypad.
        /// </summary>
        public event EventHandler<TagScanEventArgs> TagAdded;

        /// <summary>
        /// Occurs when a minifig is removed from the toypad.
        /// </summary>
        public event EventHandler<TagScanEventArgs> TagRemoved;

        /// <summary>
        /// Occurs when connected.
        /// </summary>
        public event EventHandler<ToypadEventArgs> Connected;

        /// <summary>
        /// Occurs when an led changes.
        /// </summary>
        public event EventHandler<ToypadEventArgs> LedChange;

        public event EventHandler<ToypadEventArgs> TagNotFound;
        public event EventHandler<ToypadEventArgs> TagRead;
        public event EventHandler<ToypadEventArgs> UnknownEvent;

        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        public Toypad(Dictionary<string, string> tagNames = null)
        {
            if(tagNames == null)
            {
                this.TagNames = new Dictionary<string, string>();
            }
            else
            {
                this.TagNames = tagNames;
            }
            this.KnownTags = new Dictionary<string, Tag>();
            this.PendingColourChanges = new Queue<PanelPosition>();
            this.PendingTagData = new Dictionary<int, TagScanEventArgs>();

            this.Panels = new PanelGroup(this);
        }

        /// <summary>
        /// Connects to the device
        /// </summary>
        public async Task Connect()
        {
            // already connected. Close the connection and reconnect
            if(this.Device != null)
            {
                this.Disconnect();
            }
            this.Device = new Device();
            this.Device.DeviceInitialized += Device_DeviceInitialized;
            this.Device.DeviceDisconnected += Device_DeviceDisconnected;

            this.Device.DataRecieved += Device_DataRecieved;

            await this.Device.Connect();
            this.Device.Start();
        }

        private void Device_DataRecieved(object sender, LDDeviceEventArgs e)
        {
            try
            {
                byte[] result = e.Response;
                Command command = (Command)result[2];
                switch (command)
                {
                    case Command.LedChange:
                        this.OnLedChange(result);
                        break;
                    case Command.TagNotFound:
                        TagNotFound?.Invoke(this, new ToypadEventArgs(this, result));
                        break;
                    case Command.TagScan:
                        this.OnTagScanned(result);
                        break;
                    case Command.TagRead:
                        this.OnTagRead(result);
                        break;
                    case Command.LstModel:
                        this.OnLstModel(result);
                        break;
                    case Command.Connected:
                        Connected?.Invoke(this, new ToypadEventArgs(this, result));
                        break;
                    default:
                        UnknownEvent?.Invoke(this, new ToypadEventArgs(this, result));
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void OnLedChange(byte[] result)
        {

            PanelPosition panelPos = this.PendingColourChanges.Dequeue();
            if(panelPos == PanelPosition.All)
            {
                this.Panels.Left.DequeueColor(new ToypadEventArgs(this, result));
                this.Panels.Center.DequeueColor(new ToypadEventArgs(this, result));
                this.Panels.Right.DequeueColor(new ToypadEventArgs(this, result));
            }
            else
            {
                this.Panels[panelPos].DequeueColor(new ToypadEventArgs(this, result));
            }
            LedChange?.Invoke(this, new ToypadEventArgs(this, result));
        }

        private void OnTagScanned(byte[] result)
        {
            TagScanEventArgs e = new TagScanEventArgs(this, result);
            var tag = e.Tag;

            if (string.IsNullOrEmpty(e.Uid))
            {
                throw new ArgumentException("Uid is null or empty", "Uid");
            }

            if (this.KnownTags.ContainsKey(e.Uid))
            {
                tag = this.KnownTags[e.Uid];
            }
            else
            {
                this.KnownTags.Add(e.Uid, tag);
            }
            
            if(string.IsNullOrEmpty(tag.Name) && this.TagNames.ContainsKey(e.Uid))
            {
                tag.Name = this.TagNames[e.Uid];
            }

            e.Tag = tag;

            if(tag.Type == MinifigType.Unknown)
            {
                // go see if we can get more info
                this.PendingTagData.Add(this.Device.SendPacket(new byte[] { 0x55, 0x04, 0xd2 }, new byte[] { (byte)e.Index, 0x24 }).Result, e);
                return;
            }
            else
            {
                this.SendTagData(e);
            }
        }

        private void SendTagData(TagScanEventArgs e)
        {
            switch (e.Action)
            {
                case TagAction.Add:
                    e.Panel.AddTag(e.Tag, e);
                    this.TagAdded?.Invoke(this, e);
                    break;
                case TagAction.Remove:
                    e.Panel.RemoveTag(e.Tag, e);
                    this.TagRemoved?.Invoke(this, e);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Action", (int)e.Action, typeof(TagAction));
            }

            TagScanned?.Invoke(this, e);
        }

        private void OnTagRead(byte[] result)
        {
            int counter = result[3];
            TagScanEventArgs e = this.PendingTagData[counter];
            this.PendingTagData.Remove(counter);

            e.Tag = this.KnownTags[e.Uid];

            byte isVehicle = result[14];
            if(isVehicle == 0x01)
            {
                byte[] vehicleId = new byte[] { result[5], result[6] };
                e.Tag.Character = TagDecoder.DecodeVehicle(vehicleId);
                e.Tag.Type = e.Tag.Character == Character.Unrecognized ? MinifigType.Unrecognized : MinifigType.Vehicle;
                if (e.Tag.Character != Character.Unrecognized)
                {
                    e.Tag.Name = e.Tag.Character.GetDescription();
                }
                this.SendTagData(e);
            }
            else
            {
                // we have a character, so we need even more data
                byte[] lstModel = new byte[8];
                lstModel[0] = (byte)e.Index;
                this.PendingTagData.Add(this.Device.SendPacket(new byte[] { 0x55, 0x0a, 0xd4 }, TagDecoder.Encode(lstModel)).Result, e);
                return;
            }
        }

        private void OnLstModel(byte[] result)
        {
            int counter = result[3];
            var e = this.PendingTagData[counter];
            this.PendingTagData.Remove(counter);

            e.Tag = this.KnownTags[e.Uid];

            e.Tag.Character = TagDecoder.DecodeCharacter(result.Skip(5).Take(8).ToArray());
            e.Tag.Type = e.Tag.Character == Character.Unrecognized ? MinifigType.Unrecognized : MinifigType.Character;
            if(e.Tag.Character != Character.Unrecognized)
            {
                e.Tag.Name = e.Tag.Character.GetDescription();
            }
            
            this.SendTagData(e);
        }

        private void Device_DeviceDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Device_DeviceInitialized(object sender, EventArgs e)
        {
            this.Device.Connect().Wait();
        }

        private byte[] getAllPadColor(Color color)
        {
            if (color == null) return new byte[] { 0, 0, 0, 0 };
            else return new byte[]
            {
                1,
                (byte)(color.R & 0xFF),
                (byte)(color.G & 0xFF),
                (byte)(color.B & 0xFF)
            };
        }

        public async Task PanelChangeAll(Color centerColor, Color leftColor, Color rightColor)
        {
            this.Panels.Center.PendingColors.Enqueue(centerColor);
            this.Panels.Left.PendingColors.Enqueue(leftColor);
            this.Panels.Right.PendingColors.Enqueue(rightColor);
            this.PendingColourChanges.Enqueue(PanelPosition.All);
            await this.Device.SendPacket(new byte[] { 0x55, 0x0e, 0xc8 }, new byte[0]
                .Concat(this.getAllPadColor(centerColor))
                .Concat(this.getAllPadColor(leftColor))
                .Concat(this.getAllPadColor(rightColor))
                .ToArray());

        }

        public async Task PanelChangeAll(Color color)
        {
            await this.PanelChangeAll(color, color, color);
        }

        /// <summary>
        /// Change the panel colour to the new colour instantly
        /// </summary>
        /// <param name="panel">The panel to change</param>
        /// <param name="color">The colour to change to</param>
        /// <returns></returns>
        public async Task PanelChange(PanelPosition panelPos, Color color)
        {
            if(panelPos == PanelPosition.All)
            {
                await PanelChangeAll(color);
                return;
            }
            Panel panel = this.Panels[panelPos];
            panel.PendingColors.Enqueue(color);
            this.PendingColourChanges.Enqueue(panelPos);
            await this.Device.SendPacket(new byte[] { 0x55, 0x06, 0xc0 }, new byte[]{
                    (byte)panelPos,
                    (byte)(color.R & 0xFF),
                    (byte)(color.G & 0xFF),
                    (byte)(color.B & 0xFF)
                });
        }

        /// <summary>
        /// Disconnects from the device
        /// </summary>
        public void Disconnect()
        {
            if (this.Device != null)
            {
                this.Device.Disconnect();
                this.Device.Dispose();
                this.Device = null;
            }
        }

        /// <summary>
        /// Disconnect and dispose object
        /// </summary>
        public void Dispose()
        {
            this.Disconnect();
        }
    }
}
