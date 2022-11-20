using System;

namespace Markdown
{
    public class MarkdownTag : Enumeration, ITag
    {
        public static MarkdownTag Italic = new MarkdownTag(0, nameof(Italic), "_");
        public static MarkdownTag Bold = new MarkdownTag(1, nameof(Bold), "__");
        public static Func<int, MarkdownTag> Heading = (level) 
            => new MarkdownTag(2, nameof(Heading), new string('*', level));

        public string Markup { get; }

        public MarkdownTag(int id, string name, string markup) : base(id, name)
        {
            Markup = markup;
        }
    }
}
