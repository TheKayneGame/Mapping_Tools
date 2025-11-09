using System;
using System.Collections.Generic;

namespace Mapping_Tools.Domain.ComboColourStudio
{
    public class ComboColourPoint : IComparable<ComboColourPoint>
    {
        public int CompareTo(ComboColourPoint? other)
        {
            if (other == null) return 1;
            return Time.CompareTo(other.Time);
        }
    
        public double Time { 
            get; 
            set;
        }
        /// <summary>
        /// Collection of combo colour IDs associated with this colour point.
        /// </summary>
        public SortedSet<int> ComboColourIds {
            get;
        }
        
        public ColourPointMode Mode {
            get; 
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboColourPoint"/> class with the specified time, combo colour IDs, and mode.
        /// </summary>
        public ComboColourPoint(double time, SortedSet<int> comboColourIdx, ColourPointMode mode)
        {
            Time = time;
            ComboColourIds = comboColourIdx;
            Mode = mode;
        }

        public bool AddComboColourId(int id) {
            return ComboColourIds.Add(id);
        }

        public bool RemoveComboColourId(int id) {
            return ComboColourIds.Remove(id);
        }

    }
}