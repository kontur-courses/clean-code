using System.Linq;
using Markdown.Interfaces;

namespace Markdown.LinkTags
{
    public class Link : ILinkTag
    {
        public Link()
        {
        }

        public Link(string title, string path)
        {
            Path = path;
            Title = title;
            ViewTag = $"[{title}]({path})";
        }

        public Tag Tag => Tag.Link;
        public TagType TagType => TagType.Single;
        public string Alternative => "";
        public string Title { get; }
        public string Path { get; }

        public bool TryParse(string context, int position, out ILinkTag linkTag)
        {
            linkTag = null;
            if (position != 0 || context.Length < 5 || context[^1] != ')' || context[0] != '[')
                return false;

            if (context.Count(c => c is '[' or ']' or '(' or ')') != 4)
                return false;

            var closingSquareBracketIndex = context.IndexOf(']');
            if (closingSquareBracketIndex == -1)
                return false;
            if (context[closingSquareBracketIndex + 1] != '(')
                return false;

            var title = context[1..closingSquareBracketIndex];
            var path = context[(closingSquareBracketIndex + 2)..^1];

            linkTag = new Link(title, path);
            return true;
        }

        public string ViewTag { get; }
    }
}