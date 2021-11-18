using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        public static readonly Dictionary<Type, HtmlTag> HtmlTags = new()
        {
            { typeof(BoldToken), new HtmlTag("<strong>", "\\<strong>") },
            { typeof(ItalicToken), new HtmlTag("<em>", "\\<em>") },
            { typeof(HeaderToken), new HtmlTag("<h1>", "\\<h1>") },
            { typeof(ScreeningToken), new HtmlTag(string.Empty, string.Empty) }
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

                if (text[index] != '\n')
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
                var htmlTag = HtmlTags[token.GetType()];
                result[token.OpenIndex] = new TagInsertion(htmlTag.OpenTag, token.Separator.Length);
                result[token.CloseIndex] = new TagInsertion(htmlTag.CloseTag, token.Separator.Length);
            }

            return result;
        }
    }
}