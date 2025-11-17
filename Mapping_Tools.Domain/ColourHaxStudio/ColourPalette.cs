using System;
using System.Collections.Generic;
using System.Linq;
using Mapping_Tools.Domain.Beatmaps;

namespace Mapping_Tools.Domain.ColourHaxStudio
{
    public class ColourPalette
    {
        private readonly ComboColour[] _colours;
        private int _size = 1;
        private int _maxSize = 8;
        public int Size
        {
            get => _size;
        }

        public int MaxSize
        {
            get => _maxSize;
        }

        /// <summary>
        /// Gets the list of colours in the palette.
        /// </summary>
        public IReadOnlyList<ComboColour> Colours => Array.AsReadOnly(_colours[.._size]);

        /// <summary>
        /// Constructor for ColourPalette
        /// </summary>
        /// <param name="maxSize">Maximum size of the palette</param>
        /// <param name="initialSize">Initial size of the palette</param>
        public ColourPalette(int maxSize = 8, int initialSize = 1)
        {
            _maxSize = maxSize;
            _size = initialSize;
            //palette size cannot be larger than max size and smaller than 1
            if (_size > _maxSize)
                throw new ArgumentOutOfRangeException(nameof(initialSize), "Initial size cannot be larger than maximum size.");
            if (_size < 1)
                throw new ArgumentOutOfRangeException(nameof(initialSize), "Initial size cannot be smaller than 1.");
            _colours = new ComboColour[_maxSize];

            for (int i = 0; i < initialSize; i++)
            {
                _colours[i] = new ComboColour(0, 0, 0);
            }
        }

        public bool AddColour(ComboColour colour)
        {
            if (_size >= _maxSize)
                return false;            

            if (_size < _colours.Length)
            {
                _colours[_size] = colour;
            }
            _size++;
            return true;
        }

        public bool AddColour()
        {
            // ensure we don't exceed max size
            if (_size >= _maxSize)
                return false;
            _size++;
            return true;
        }

        public bool RemoveColour()
        {
            if (_size <= 1)
                return false;
            _size--;
            return true;
        }

        public void SetColour(int index, ComboColour colour)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the current palette size.");
            _colours[index] = colour;
        }




    }
}
