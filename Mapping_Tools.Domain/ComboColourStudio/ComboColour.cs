using System;

namespace Mapping_Tools.Domain.ComboColourStudio
{
    /// <summary>
    /// Entity representing a palette entry with unique identifier and value object colour.
    /// </summary>
    public class ComboColour
    {
        public Guid Id { get; }
        public Colour Colour {
            get; 
        }

        public ComboColour(Guid id, Colour colour)
        {
            Id = id;
            Colour = colour;
        }
    }
}