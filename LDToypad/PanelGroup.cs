using System;
using System.Collections.Generic;
using System.Text;

namespace LDToypad
{
    public class PanelGroup
    {
        /// <summary>
        /// Gets or sets the panels.
        /// </summary>
        private Dictionary<PanelPosition, Panel> panels { get; set; }

        /// <summary>
        /// Gets the <see cref="Panel"/> with the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The <see cref="Panel"/>.</returns>
        public Panel this[PanelPosition position]
        {
            get { return panels[position]; }
        }

        /// <summary>
        /// Gets the left panel.
        /// </summary>
        public Panel Left { get { return panels[PanelPosition.Left]; } }

        /// <summary>
        /// Gets the center panel.
        /// </summary>
        public Panel Center { get { return panels[PanelPosition.Center]; } }

        /// <summary>
        /// Gets the right panel.
        /// </summary>
        public Panel Right { get { return panels[PanelPosition.Right]; } }

        /// <summary>
        /// Gets or sets the toypad.
        /// </summary>
        internal Toypad Toypad { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelGroup"/> class.
        /// </summary>
        public PanelGroup(Toypad toypad)
        {
            this.Toypad = toypad;
            this.panels = new Dictionary<PanelPosition, Panel>
            {
                { PanelPosition.Left, new Panel(PanelPosition.Left, toypad) },
                { PanelPosition.Center, new Panel(PanelPosition.Center, toypad) },
                { PanelPosition.Right, new Panel(PanelPosition.Right, toypad) }
            };
        }
    }
}
