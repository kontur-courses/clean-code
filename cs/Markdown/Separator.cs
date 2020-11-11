namespace Markdown
{
    internal class Separator
    {
        public readonly int PositionInText;
        public readonly string Value;

        public Separator(string value, int position)
        {
            Value = value;
            PositionInText = position;
        }
    }
}