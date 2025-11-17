using System;
using System.Collections.Generic;

namespace Mapping_Tools.Domain.ColourHaxStudio
{
    public class ColourHaxPoint : IComparable<ColourHaxPoint>
    {

        private List<int> _sequence;
        /// <summary>
        /// The time position of this colour point in seconds.
        /// This is the Identifier for the colour point.
        /// </summary>
        public double Time { 
            get; 
            set;
        }
        /// <summary>
        /// Collection of combo colour IDs associated with this colour point.
        /// </summary>
        public List<int> Sequence {
            get => _sequence;
        }
        
        public ColourPointMode Mode {
            get; 
            set;
        }

        /// <summary>
        /// Constructor for ColourHaxPoint
        /// </summary>
        /// <param name="time">Time indicating moment</param>
        /// <param name="sequence">List of combo colour IDs</param>
        /// <param name="mode">Colour point mode</param>
        public ColourHaxPoint(double time, List<int> sequence, ColourPointMode mode)
        {
            Time = time;
            _sequence = sequence;
            Mode = mode;
        }

        /// <summary>
        /// Constructor for ColourHaxPoint with empty sequence
        /// </summary>
        /// <param name="time">Time indicating moment</param>
        /// <param name="mode">Colour point mode</param>
        public ColourHaxPoint(double time, ColourPointMode mode)
        {
            Time = time;
            _sequence = new List<int>();
            Mode = mode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void AddPaletteId(int id) {
            _sequence.Add(id);
        }

        /// <summary>
        /// Adds a combo colour ID at a specific index in the sequence.
        /// </summary>
        /// <param name="index">Index at which to insert the ID</param>
        /// <param name="id">ID of the combo colour to add</param>
        /// <returns>Whether the addition was successful</returns>
        public bool AddPaletteIdAt(int index, int id) {
            if (index < 0 || index > _sequence.Count) return false;
            _sequence.Insert(index, id);
            return true;
        }

        /// <summary>
        /// Removes the last palette colour ID from the sequence.
        /// </summary>
        /// <returns>Whether the removal was successful</returns>
        public bool RemovePaletteId() 
            {
            if (_sequence.Count == 0) return false;
            _sequence.RemoveAt(_sequence.Count - 1);
            return true;
        }

        /// <summary>
        /// Removes a palette colour ID at a specific index in the sequence.
        /// </summary>
        /// <param name="index">Index of the palette colour to remove</param>
        /// <returns>Whether the removal was successful</returns>
        public bool RemovePaletteIdAt(int index) {
            if (index < 0 || index >= _sequence.Count) return false;
            _sequence.RemoveAt(index);
            return true;
        }

        public void SetPaletteSequence(List<int> ids) {
            _sequence = ids;
        }

        public void ClearPaletteSequence() {
            _sequence.Clear();
        }

        public int CompareTo(ColourHaxPoint? other)
        {
            if (other == null) return 1;
            return Time.CompareTo(other.Time);
        }

    }
}