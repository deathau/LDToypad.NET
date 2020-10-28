using System;
using System.Threading;
using System.Threading.Tasks;
using Device.Net;
using System.Linq;
using LDToypad;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

#if (!LIBUSB)
using Usb.Net.Windows;
using Hid.Net.Windows;
//using SerialPort.Net.Windows;
#else
using Device.Net.LibUsb;
#endif

namespace Example
{
    class Program
    {
        #region Fields
        private static ManualResetEvent reset = new ManualResetEvent(false);
        private static Dictionary<string, Tag> minifigsByUid = new Dictionary<string, Tag>();
        #endregion

        #region Main
        private static void Main(string[] args)
        {
            DoTheTest();
            reset.WaitOne();
        }

        private static async Task DoTheTest()
        {
            var test = new Toypad(MinifigUidLookup);
            try
            {
                Console.WriteLine("Connecting");
                await test.Connect();

                test.TagScanned += Toypad_MinifigScanned;
                test.Connected += Toypad_Connected;
                test.TagNotFound += Toypad_TagNotFound;
                test.TagRead += Toypad_TagRead;
                test.LedChange += Toypad_LedChange;
                test.UnknownEvent += Toypad_UnknownEvent;

                await Task.Delay(250);

                await test.PanelChangeAll(Color.Red, Color.Green, Color.Blue);
                await Task.Delay(1000);
                await test.PanelChange(PanelPosition.Center, Color.Magenta);
                await Task.Delay(1000);
                await test.PanelChange(PanelPosition.Left, Color.Yellow);
                await Task.Delay(1000);
                await test.PanelChange(PanelPosition.Right, Color.Cyan);

                Console.WriteLine("Connected. Press any key to quit...");
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                test.Disconnect();
                test.Dispose();
                Console.WriteLine("end");
                await Task.Delay(1000);
                reset.Set();
            }
        }
        #endregion

        private static void Toypad_UnknownEvent(object sender, ToypadEventArgs e)
        {
            Console.WriteLine("[UNKNOWN] " + string.Join(' ', e.Response.Select(x => x.ToString("X2"))));
        }

        private static void Toypad_LedChange(object sender, ToypadEventArgs e)
        {
            Console.WriteLine("[LedChange] " + string.Join(' ', e.Response.Select(x => x.ToString("X2"))));
        }

        private static void Toypad_TagRead(object sender, ToypadEventArgs e)
        {
            Console.WriteLine("[TagRead] " + string.Join(' ', e.Response.Select(x => x.ToString("X2"))));
        }

        private static void Toypad_TagNotFound(object sender, ToypadEventArgs e)
        {
            Console.WriteLine("[TagNotFound] " + string.Join(' ', e.Response.Select(x => x.ToString("X2"))));
        }

        private static void Toypad_Connected(object sender, ToypadEventArgs e)
        {
            Console.WriteLine("[Connected] " + string.Join(' ', e.Response.Select(x => x.ToString("X2"))));

        }

        private static void Toypad_MinifigScanned(object sender, TagScanEventArgs e)
        {
            Tag minifig = e.Tag;
            if(minifigsByUid.ContainsKey(minifig.Uid))
            {
                int index = minifig.Index;
                minifig = minifigsByUid[minifig.Uid];
                minifig.Index = index;
            }
            else
            {
                if (MinifigUidLookup.ContainsKey(minifig.Uid))
                {
                    minifig.Name = MinifigUidLookup[minifig.Uid];
                }

                minifigsByUid.Add(minifig.Uid, minifig);
            }

            Console.WriteLine($"-- Minifig {(e.Action == TagAction.Add ? "Added" : "Removed")}: {minifig.Name} [{minifig.Character}] ");
            if (minifig.Name == "Batman") // Batman
                e.Panel.Change(e.Action == TagAction.Add ? Color.FromArgb(255, 100, 100) : Color.Black).Wait();
            else if (minifig.Name == "Wildstyle") // Wildstyle
                e.Panel.Change(e.Action == TagAction.Add ? Color.Blue : Color.Black).Wait();
            else if (minifig.Name == "Batgirl") // Batgirl
                e.Panel.Change(e.Action == TagAction.Add ? Color.FromArgb(255, 200, 0) : Color.Black).Wait();
        }


        private static Dictionary<string, string> MinifigUidLookup = new Dictionary<string, string>
        {
            // These are the UIDs for *my* minifigs and won't match yours.
            // You can add UIDs of other NFC tags here, too.
            { "A1 BD 4A 0B 40 80", "Batman" },
            { "74 D2 1A 6E 40 80", "Wildstyle" },
            { "55 D8 92 31 4D 81", "Batgirl" }
        };
    }
}
