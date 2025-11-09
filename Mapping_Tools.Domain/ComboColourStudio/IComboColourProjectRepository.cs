namespace Mapping_Tools.Domain.ComboColourStudio
{
    public interface IComboColourProjectRepository
    {
        ComboColourStudioProject Load(string path);
        void Save(ComboColourStudioProject project, string path);
    }
}