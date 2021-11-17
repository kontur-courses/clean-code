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
            {typeof(BoldToken), new HtmlTag("<strong>", "\\<strong>")},
            {typeof(ItalicToken), new HtmlTag("<em>", "\\<em>")},
            {typeof(HeaderToken), new HtmlTag("<h1>", "\\<h1>")},
            {typeof(ScreeningToken), new HtmlTag(string.Empty, string.Empty)},
        };

        public string Render(IEnumerable<Token> tokens, string text)
        {
            var textReplacements = GetTextReplacements(tokens);
            var result = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                    continue;
                if (textReplacements.TryGetValue(i, out var replacement))
                {
                    result.Append(replacement.Tag);
                    i += replacement.Shift - 1;
                }
                else
                {
                    result.Append(text[i]);
                }
            }

            return result.ToString();
        }

        private static Dictionary<int, TextReplacement> GetTextReplacements(IEnumerable<Token> tokens)
        {
            var result = new Dictionary<int, TextReplacement>();

            foreach (var token in tokens)
            {
                var htmlTag = HtmlTags[token.GetType()];
                result[token.OpenIndex] = new TextReplacement(htmlTag.OpenTag, token.Separator.Length);
                result[token.CloseIndex] =  new TextReplacement(htmlTag.CloseTag, token.Separator.Length);
            }

            return result;
        }

    }
}