using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDToypad
{
    /// <summary>
    /// Event args emitted from a minifig scan event
    /// </summary>
    /// <seealso cref="LDToypad.ToypadEventArgs" />
    public class TagScanEventArgs : PanelEventArgs
    {
        /// <summary>
        /// Gets the action (added or removed).
        /// </summary>
        public TagAction Action { get; private set; }

        /// <summary>
        /// Gets the uid.
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Index { get; private set; }

        public int Counter { get; private set; }

        /// <summary>
        /// Gets the minifig data.
        /// </summary>
        public Tag Tag { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagScanEventArgs"/> class.
        /// </summary>
        /// <param name="toypad">The toypad.</param>
        /// <param name="response">The response.</param>
        public TagScanEventArgs(Toypad toypad, byte[] response) : base(toypad, response)
        {
            this.Position = (PanelPosition)response[3];
            this.Panel = toypad.Panels[this.Position];
            this.Counter = response[4];
            this.Index = response[5];
            this.Action = (TagAction)response[6];
            this.Uid = Tag.BufferToUid(response.Skip(8).Take(6));
            this.Tag = new Tag(this.Uid)
            {
                Index = this.Index
            };
        }

        
    }
}
