using Markdown.Enums;

namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public string Opening { get; }
        public string Closing { get; }

        public Tag(string opening, string closing)
        {
            Opening = opening;
            Closing = closing;
        }
    }
}