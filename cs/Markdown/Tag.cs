namespace Markdown
{
    public class Tag
    {
        public readonly string TegName;
        public readonly int Position;

        public Tag(string tegName, int position)
        {
            TegName = tegName;
            Position = position;
        }


        public string BuildHtmlTag(bool isClosingTeg)
        {
            throw new System.NotImplementedException();
        }
    }
}