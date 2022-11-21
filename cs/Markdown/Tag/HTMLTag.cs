using System;

namespace Markdown
{
    public class HTMLTag : ITag
    {
        public static HTMLTag Strong = new HTMLTag(0, nameof(Strong), "strong");
        public static HTMLTag Emphasys = new HTMLTag(1, nameof(Emphasys), "em");
        public static Func<int, HTMLTag> Heading = (level) 
            => new HTMLTag(0, nameof(Heading), $"h{level}");

        public string Markup { get; }

        public HTMLTag(int id, string name, string markup)
        {
            Markup = markup;
        }
    }
}
