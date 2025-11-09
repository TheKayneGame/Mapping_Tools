namespace Mapping_Tools.Domain.ComboColourStudio
{
    /// <summary>
    /// Value Object representing an immutable RGB colour.
    /// </summary>
    public sealed class Colour
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public Colour(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object obj)
        {
            if (obj is Colour other)
            {
                return R == other.R && G == other.G && B == other.B;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (R, G, B).GetHashCode();
        }
    }
}