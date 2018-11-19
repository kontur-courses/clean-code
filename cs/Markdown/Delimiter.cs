namespace Markdown
{
    public class Delimiter
    {
        public readonly bool CanBeClosing;
        public readonly bool CanBeStarting;
        public readonly int Index;
        public readonly string Value;

        public Delimiter(string value, int index, bool canBeClosing, bool canBeStarting)
        {
            Value = value;
            Index = index;
            CanBeClosing = canBeClosing;
            CanBeStarting = canBeStarting;
        }
    }
}