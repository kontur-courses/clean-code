using System.Linq;
using Markdown.Interfaces;

namespace Markdown.LinkTags
{
    public class Image : ILinkTag
    {
        public Image()
        {
        }

        public Image(string alt, string path)
        {
            Alternative = alt;
            Path = path;
            ViewTag = $"![{alt}]({path})";
        }

        public Tag Tag => Tag.Image;
        public TagType TagType => TagType.Single;
        public string Alternative { get; }
        public string Title => "";
        public string Path { get; }

        public bool TryParse(string context, int position, out ILinkTag linkTag)
        {
            linkTag = null;
            if (position != 0 || context.Length < 6 ||
                context[0] != '!' || context[1] != '[' || context[^1] != ')')
                return false;
            if (context.Count(c => c is '[' or ']' or '(' or ')') != 4)
                return false;

            var closingSquareBracketIndex = context.IndexOf(']');
            if (closingSquareBracketIndex == -1)
                return false;
            if (context[closingSquareBracketIndex + 1] != '(')
                return false;

            var alternative = context[2..closingSquareBracketIndex];
            var path = context[(closingSquareBracketIndex + 2)..^1];

            linkTag = new Image(alternative, path);
            return true;
        }

        public string ViewTag { get; }
    }
}