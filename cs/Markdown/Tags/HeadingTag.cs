using System.Collections.Generic;

namespace Markdown
{
    public class HeadingTag : ITag
    {
        public string OpeningMarkup => "# ";
        public string ClosingMarkup => "\n";
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";

        public void Replace(List<string> builder, int start, int end)
        {
            builder[start] = OpeningTag;
            builder[start + 1] = "";
            if (builder[end] == "\n")
                builder.Insert(end, ClosingTag);
            else
                builder.Insert(end + 1, ClosingTag);
        }

        public bool IsBrokenMarkup(string source, int start, int length)
        {
            return !HasOpeningMarkup(source, start);
        }

        private bool HasOpeningMarkup(string source, int start)
        {
            return source.Length > start + 1
                   && source[start] == OpeningMarkup[0]
                   && source[start + 1] == OpeningMarkup[1];
        }
    }
}