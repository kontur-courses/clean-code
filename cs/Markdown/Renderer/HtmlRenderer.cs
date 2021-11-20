using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        public static readonly Dictionary<string, HtmlTag> HtmlTags = new()
        {
            { "__", new HtmlTag("<strong>", "\\<strong>") },
            { "_", new HtmlTag("<em>", "\\<em>") },
            { "#", new HtmlTag("<h1>", "\\<h1>") },
            { "\\", new HtmlTag(string.Empty, string.Empty) }
        };

        public string Render(IEnumerable<Token> tokens, string text)
        {
            var tagInsertions = GetTagInsertions(tokens);
            var result = new StringBuilder();
            var index = 0;

            while (index < text.Length)
            {
                if (tagInsertions.TryGetValue(index, out var replacement))
                {
                    result.Append(replacement.Tag);
                    index += replacement.Shift;
                    continue;
                }

                result.Append(text[index]);

                index++;
            }

            if (tagInsertions.TryGetValue(text.Length, out var endTag))
                result.Append(endTag.Tag);

            return result.ToString();
        }

        private static Dictionary<int, TagInsertion> GetTagInsertions(IEnumerable<Token> tokens)
        {
            var result = new Dictionary<int, TagInsertion>();

            foreach (var token in tokens)
            {
                var htmlTag = HtmlTags[token.GetSeparator()];
                result[token.OpenIndex] = new TagInsertion(htmlTag.OpenTag, token.GetSeparator().Length);
                result[token.CloseIndex] = new TagInsertion(htmlTag.CloseTag, token.GetSeparator().Length);
            }

            return result;
        }
    }
}