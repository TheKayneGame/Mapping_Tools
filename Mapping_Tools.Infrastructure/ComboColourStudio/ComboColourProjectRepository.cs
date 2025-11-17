using Mapping_Tools.Domain.ColourHaxStudio;
using System.Text.Json;

namespace Mapping_Tools.Infrastructure.ComboColourStudio
{
    public class ComboColourProjectRepository : IColourHaxProjectRepository
    {
        public ColourHaxProject Load(string path)
        {
            var json = File.ReadAllText(path);
            var project = JsonSerializer.Deserialize<ColourHaxProject>(json);
            if (project is null)
            {
                throw new InvalidOperationException("Failed to deserialize ComboColourStudioProject from file.");
            }
            return project;
        }

        public void Save(ColourHaxProject project, string path)
        {
            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}