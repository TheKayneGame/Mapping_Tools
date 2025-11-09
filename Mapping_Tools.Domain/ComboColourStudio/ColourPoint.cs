namespace Mapping_Tools.Domain.ComboColourStudio
{
    public class ColourPoint
    {
        public double Time { get; }
        public List<Guid> ColourSequenceIds { get; }
        public ColourPointMode Mode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourPoint"/> class with the specified time, colour sequence,
        /// and mode.
        /// </summary>
        /// <param name="time">The time associated with this colour point, typically representing a position in a timeline or sequence.</param>
        /// <param name="colours">The sequence of colours to associate with this colour point. Cannot be null.</param>
        /// <param name="mode">The mode that determines how the colour sequence is interpreted or applied.</param>
        public ColourPoint(double time, IEnumerable<Guid> colourIds, ColourPointMode mode)
        {
            Time = time;
            ColourSequenceIds = new List<Guid>(colourIds);
            Mode = mode;
        }
    }
}