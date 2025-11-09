using System;
using System.Collections.Generic;
using System.Linq;
using Mapping_Tools.Domain.Beatmaps;

namespace Mapping_Tools.Domain.ComboColourStudio
{
    public class ComboColourStudioProject
    {
        public event EventHandler<Guid>? PaletteColourChanged;
        private readonly ColourPalette colourPalette = new();
        private readonly SortedSet<ComboColourPoint> _comboColourPoints = new();


        public IReadOnlyCollection<ComboColourPoint> ComboColourPoints
        {
            get => _comboColourPoints;
        }
        public int MaxBurstLength { get; private set; }
        public ColourPalette ColourPalette
        {
            get => colourPalette;
        }

        // Colour Point Management
        public void AddColourPoint(ComboColourPoint colourPoint)
        {
            _comboColourPoints.Add(colourPoint);
        }

        public void RemoveColourPoint(ComboColourPoint colourPoint)
        {
            _comboColourPoints.Remove(colourPoint);
        }

        public void RemoveColourPoint(int index)
        {
            if (index < 0 || index >= _comboColourPoints.Count) return;

            var item = _comboColourPoints.ElementAt(index);
            _comboColourPoints.Remove(item);
        }

        public ComboColourPoint? GetColourPoint(int index)
        {
            if (index < 0 || index >= _comboColourPoints.Count)
                return null;
            return _comboColourPoints.ElementAt(index);
        }

        public bool UpdateColourPoint(int index, ComboColourPoint newPoint)
        {
            if (index < 0 || index >= _comboColourPoints.Count)
                return false;
            var oldPoint = _comboColourPoints.ElementAt(index);
            _comboColourPoints.Remove(oldPoint);
            _comboColourPoints.Add(newPoint);
            return true;
        }

        // ComboColourId Management in Colour Points
        public bool AddComboColourIdToPoint(int pointIndex, int id)
        {
            var point = GetColourPoint(pointIndex);
            return point != null && point.AddComboColourId(id);
        }

        public bool RemoveComboColourIdFromPoint(int pointIndex, int id)
        {
            var point = GetColourPoint(pointIndex);
            return point != null && point.RemoveComboColourId(id);
        }

        // Palette Management
        public bool AddColourToPalette(ComboColour colour)
        {
            return colourPalette.AddColour(colour);
        }

        public bool AddDefaultColourToPalette()
        {
            return colourPalette.AddColour();
        }

        public bool RemoveColourFromPalette()
        {
            return colourPalette.RemoveColour();
        }

        public void SetPaletteColour(int index, ComboColour colour)
        {
            colourPalette.SetColour(index, colour);
        }
    }
}