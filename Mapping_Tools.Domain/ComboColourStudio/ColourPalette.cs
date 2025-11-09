using System;
using System.Collections.Generic;
using System.Linq;
using Mapping_Tools.Domain.Beatmaps;

namespace Mapping_Tools.Domain.ComboColourStudio
{
    public class ColourPalette
    {
        private readonly List<ComboColour> _colours = new();
        private int _size = 1;


        public IReadOnlyList<ComboColour> Colours => _colours.AsReadOnly();
        public int Size
        {
            get;
        }

        public ColourPalette()
        {
            _colours.Add(new ComboColour(255,255,255));
        }

        public bool AddColour(ComboColour colour)
        {
            if (_colours.Count >= 8)
                return false;            

            if (_colours.Count < _size)
            {
                _colours.Add(colour);
            }
            _size++;
            return true;
        }

        public bool AddColour()
        {
            return AddColour(new ComboColour(255, 255, 255));
        }

        public bool RemoveColour()
        {
            _size--;
            return true;
        }

        public void SetColour(int index, ComboColour colour)
        {
            if (index < 0 || index >= _colours.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid colour index.");
            _colours[index] = colour;
        }




    }
}
