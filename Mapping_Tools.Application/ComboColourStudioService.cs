using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapping_Tools.Domain.Beatmaps;
using Mapping_Tools.Domain.ComboColourStudio;

namespace Mapping_Tools.Application
{
    public class ComboColourStudioService
    {
        private readonly IComboColourProjectRepository _projectRepository;

        public ComboColourStudioProject? CurrentProject { get; private set; }

        public ComboColourStudioService(IComboColourProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // Project Management
        public void LoadProject(string projectPath)
        {
            CurrentProject = _projectRepository.Load(projectPath);
        }

        public void SaveProject(string projectPath)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            _projectRepository.Save(CurrentProject, projectPath);
        }

        // Colour Point Management
        public void AddColourPoint(double time, SortedSet<int>? comboColourIds = null, ColourPointMode mode = ColourPointMode.Normal)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            var point = new ComboColourPoint(time, comboColourIds ?? new SortedSet<int>(), mode);
            CurrentProject.AddColourPoint(point);
        }

        public void RemoveColourPoint(int index)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            CurrentProject.RemoveColourPoint(index);
        }

        public ComboColourPoint? GetColourPoint(int index)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.GetColourPoint(index);
        }

        public bool UpdateColourPoint(int index, ComboColourPoint newPoint)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.UpdateColourPoint(index, newPoint);
        }

        // ComboColourId Management in Colour Points
        public bool AddComboColourIdToPoint(int pointIndex, int id)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.AddComboColourIdToPoint(pointIndex, id);
        }

        public bool RemoveComboColourIdFromPoint(int pointIndex, int id)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.RemoveComboColourIdFromPoint(pointIndex, id);
        }

        // Palette Management
        public bool AddColourToPalette(ComboColour colour)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.AddColourToPalette(colour);
        }

        public bool AddDefaultColourToPalette()
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.AddDefaultColourToPalette();
        }

        public bool RemoveColourFromPalette()
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            return CurrentProject.RemoveColourFromPalette();
        }

        public void SetPaletteColour(int index, ComboColour colour)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("No project is currently loaded.");
            CurrentProject.SetPaletteColour(index, colour);
        }

        // Additional methods for exporting/importing can be added here as needed
    }
}
