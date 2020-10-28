using System.Collections.Generic;
using System.Linq;

namespace LDToypad
{
    /// <summary>
    /// Represents a minifig (as the result of an action, usually)
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the index of this minifig from the toypad.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the uid of the minifig.
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets or sets a human-readable name for this minifig.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public MinifigType Type { get; internal set; }

        /// <summary>
        /// Gets the character.
        /// </summary>
        public Character Character { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public Tag(string uid)
        {
            this.Uid = uid;
        }

        /// <summary>
        /// Converts a byte buffer to a uid string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A UID string.</returns>
        public static string BufferToUid(IEnumerable<byte> buffer)
        {
            return string.Join(' ', buffer.Select(x => x.ToString("X2")));
        }
    }

}
