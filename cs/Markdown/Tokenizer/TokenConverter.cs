using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class TokenConverter
    {
        public static string ConvertToHtml(IEnumerable<Token> tokens)
        {
            var str = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Type != TagType.None)
                {
                    var (start, end) = MarkdownTagsLibrary.TagToHtmlTagAssociations[token.Type];
                    str.Append(start);
                    str.Append(token.Text);
                    str.Append(end);
                }
                else
                    str.Append(token.Text);
            }

            return str.ToString();
        }
    }
}