using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDToypad
{
    /// <summary>
    /// Represents one of the three panels of the toypad.
    /// Used to keep track of the connected Minifigures, etc.
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Gets the position identifier of the panel.
        /// </summary>
        public PanelPosition Position { get; private set; }

        /// <summary>
        /// Gets the list of minifigs currently on this panel.
        /// </summary>
        public Dictionary<string, Tag> Tags { get; private set; }

        /// <summary>
        /// Gets or sets the toypad.
        /// </summary>
        internal Toypad Toypad { get; set; }

        /// <summary>
        /// Gets or sets the current color of the LED(s).
        /// </summary>
        internal Color CurrentColor { get; set; }

        /// <summary>
        /// Gets or sets the pending colors.
        /// </summary>
        internal Queue<Color> PendingColors { get; set; }

        /// <summary>
        /// Occurs when a minifig is added to the panel.
        /// </summary>
        public event EventHandler<TagScanEventArgs> TagAdded;

        /// <summary>
        /// Occurs when a minifig is removed from the panel.
        /// </summary>
        public event EventHandler<TagScanEventArgs> TagRemoved;

        /// <summary>
        /// Occurs when the led colour is changed.
        /// </summary>
        public event EventHandler<LedChangeEventArgs> LedChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Panel(PanelPosition position, Toypad toypad)
        {
            this.Position = position;
            this.Toypad = toypad;
            this.Tags = new Dictionary<string, Tag>();
            this.PendingColors = new Queue<Color>();
            this.CurrentColor = Color.Black;
        }

        /// <summary>
        /// Adds the minifig.
        /// </summary>
        /// <param name="tag">The minifig.</param>
        public void AddTag(Tag tag, TagScanEventArgs e)
        {
            if (this.Tags.ContainsKey(tag.Uid))
            {
                this.Tags[tag.Uid] = tag;
            }
            else
            {
                this.Tags.Add(tag.Uid, tag);
            }

            e.Tag = tag;
            this.TagAdded?.Invoke(e.Toypad, e);
        }

        /// <summary>
        /// Removes the minifig.
        /// </summary>
        /// <param name="tag">The minifig.</param>
        public void RemoveTag(Tag tag, TagScanEventArgs e)
        {
            if (this.Tags.ContainsKey(tag.Uid))
            {
                this.Tags.Remove(tag.Uid);
            }

            e.Tag = tag;
            this.TagRemoved?.Invoke(e.Toypad, e);
        }

        /// <summary>
        /// Removes the minifig.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public void RemoveTag(string uid, TagScanEventArgs e)
        {
            if (this.Tags.ContainsKey(uid))
            {
                this.Tags.Remove(uid);
            }

            this.TagRemoved?.Invoke(e.Toypad, e);
        }

        /// <summary>
        /// Changes the LEDs to the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        public async Task Change(Color color)
        {
            await this.Toypad.PanelChange(this.Position, color);
        }

        /// <summary>
        /// Dequeues the color.
        /// </summary>
        /// <param name="e">The <see cref="ToypadEventArgs"/> instance containing the event data.</param>
        internal void DequeueColor(ToypadEventArgs e)
        {
            this.CurrentColor = this.PendingColors.Dequeue();
            this.LedChanged?.Invoke(this, new LedChangeEventArgs(e.Toypad, e.Response, this.CurrentColor, this));
        }
    }
}
