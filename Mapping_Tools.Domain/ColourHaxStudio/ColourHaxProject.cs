using System;
using System.Collections.Generic;
using System.Linq;
using Mapping_Tools.Domain.Beatmaps;

namespace Mapping_Tools.Domain.ColourHaxStudio
{
    public class ColourHaxProject
    {
        private readonly ColourPalette colourPalette = new();
        private readonly SortedSet<ColourHaxPoint> _comboColourPoints = new();


        public IReadOnlyCollection<ColourHaxPoint> ComboColourPoints
        {
            get => _comboColourPoints;
        }
        public int MaxBurstLength { get; private set; }
        public ColourPalette ColourPalette
        {
            get => colourPalette;
        }

        // Colour Point Management
        public void AddColourHaxPoint(ColourHaxPoint colourPoint)
        {
            _comboColourPoints.Add(colourPoint);
        }

        public void RemoveColourHaxPoint(ColourHaxPoint colourPoint)
        {
            _comboColourPoints.Remove(colourPoint);
        }

        public void RemoveColourHaxPoint(int index)
        {
            if (index < 0 || index >= _comboColourPoints.Count) return;

            var item = _comboColourPoints.ElementAt(index);
            _comboColourPoints.Remove(item);
        }

        public ColourHaxPoint? GetColourHaxPoint(int index)
        {
            if (index < 0 || index >= _comboColourPoints.Count)
                return null;
            return _comboColourPoints.ElementAt(index);
        }

        public bool UpdateColourHaxPoint(int index, ColourHaxPoint newPoint)
        {
            if (index < 0 || index >= _comboColourPoints.Count)
                return false;
            var oldPoint = _comboColourPoints.ElementAt(index);
            _comboColourPoints.Remove(oldPoint);
            _comboColourPoints.Add(newPoint);
            return true;
        }

        // PaletteId Management in Colour Points
        public bool AddPaletteIdToPoint(int pointIndex, int id)
        {
            var point = GetColourHaxPoint(pointIndex);
            return point != null && AddPaletteIdToPointAt(pointIndex, point.Sequence.Count, id);
        }

        public bool AddPaletteIdToPointAt(int pointIndex, int sequenceIndex, int id)
        {
            var point = GetColourHaxPoint(pointIndex);
            return point != null && point.AddPaletteIdAt(sequenceIndex, id);
        }

        public bool RemovePaletteIdFromPointAt(int pointIndex, int sequenceIndex)
        {
            var point = GetColourHaxPoint(pointIndex);
            return point != null && point.RemovePaletteIdAt(sequenceIndex);
        }

        public bool RemovePaletteIdFromPoint(int pointIndex)
        {
            var point = GetColourHaxPoint(pointIndex);
            return point != null && point.RemovePaletteId();
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