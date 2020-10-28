using System;
using System.Collections.Generic;
using System.Text;

namespace LDToypad
{
    public class PanelEventArgs : ToypadEventArgs
    {
        /// <summary>
        /// Gets the panel the action took place on.
        /// </summary>
        public PanelPosition Position { get; internal set; }

        public Panel Panel { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagScanEventArgs"/> class.
        /// </summary>
        /// <param name="toypad">The toypad.</param>
        /// <param name="response">The response.</param>
        internal PanelEventArgs(Toypad toypad, byte[] response) : base(toypad, response)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagScanEventArgs"/> class.
        /// </summary>
        /// <param name="toypad">The toypad.</param>
        /// <param name="response">The response.</param>
        public PanelEventArgs(Toypad toypad, byte[] response, PanelPosition position) : base(toypad, response)
        {
            this.Position = position;
            this.Panel = toypad.Panels[position];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagScanEventArgs"/> class.
        /// </summary>
        /// <param name="toypad">The toypad.</param>
        /// <param name="response">The response.</param>
        public PanelEventArgs(Toypad toypad, byte[] response, Panel panel) : base(toypad, response)
        {
            this.Position = panel.Position;
            this.Panel = panel;
        }
    }
}
