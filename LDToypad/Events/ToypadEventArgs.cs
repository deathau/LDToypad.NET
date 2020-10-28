using System;
using System.Collections.Generic;
using System.Text;

namespace LDToypad
{
    /// <summary>
    /// Event args for toypad events
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ToypadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the toypad which emitted the event.
        /// </summary>
        public Toypad Toypad { get; private set; }

        /// <summary>
        /// Gets the raw response data from the event.
        /// </summary>
        public byte[] Response { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToypadEventArgs"/> class.
        /// </summary>
        /// <param name="toypad">The toypad.</param>
        /// <param name="response">The response.</param>
        public ToypadEventArgs(Toypad toypad, byte[] response) : base()
        {
            this.Toypad = toypad;
            this.Response = response;
        }
    }
}
