using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LDToypad
{
    public class LedChangeEventArgs : PanelEventArgs
    {
        public Color Color { get; private set; }
        public LedChangeEventArgs(Toypad toypad, byte[] response, Color color, Panel panel) : base(toypad, response, panel)
        {
            this.Color = color;
        }
    }
}
