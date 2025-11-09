using Mapping_Tools.Domain.ComboColourStudio;
using System.Text.Json;

namespace Mapping_Tools.Infrastructure.ComboColourStudio
{
    public class ComboColourProjectRepository : IComboColourProjectRepository
    {
        public ComboColourStudioProject Load(string path)
        {
            var json = File.ReadAllText(path);
            var project = JsonSerializer.Deserialize<ComboColourStudioProject>(json);
            if (project is null)
            {
                throw new InvalidOperationException("Failed to deserialize ComboColourStudioProject from file.");
            }
            return project;
        }

        public void Save(ComboColourStudioProject project, string path)
        {
            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}