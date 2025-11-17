namespace Mapping_Tools.Domain.ColourHaxStudio
{
    public interface IColourHaxProjectRepository
    {
        ColourHaxProject Load(string path);
        void Save(ColourHaxProject project, string path);
    }
}