using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        public readonly IReadOnlyDictionary<string, HtmlTag> HtmlTagsBySeparator;

        public HtmlRenderer(IReadOnlyDictionary<string, HtmlTag> htmlTagsBySeparators)
        {
            HtmlTagsBySeparator = htmlTagsBySeparators;
        }

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

                    if (replacement.Shift > 0)
                        continue;
                }

                result.Append(text[index]);
                index++;
            }

            if (tagInsertions.TryGetValue(text.Length, out var endTag))
                result.Append(endTag.Tag);

            return result.ToString();
        }

        private Dictionary<int, TagInsertion> GetTagInsertions(IEnumerable<Token> tokens)
        {
            var result = new Dictionary<int, TagInsertion>();

            foreach (var token in tokens)
            {
                var htmlTag = HtmlTagsBySeparator[token.GetSeparator()];

                if (token.IsContented)
                    result[token.OpenIndex] = GetContentedTokenInsertion(token, htmlTag);
                else
                    result[token.OpenIndex] = new TagInsertion(htmlTag.OpenTag, token.GetSeparator().Length);

                if (htmlTag.IsPaired)
                    result[token.CloseIndex] = new TagInsertion(htmlTag.CloseTag, token.IsNonPaired ? 0 : token.GetSeparator().Length);
            }

            return result;
        }

        private static TagInsertion GetContentedTokenInsertion(Token token, HtmlTag htmlTag)
        {
            var altText = token.AltText.Length > 0 ? $" alt=\"{token.AltText}\"" : string.Empty;
            var source = $"src=\"{token.Source}\"";
            var insertion = htmlTag.OpenTag.Insert(htmlTag.OpenTag.Length - 1, $"{source}{altText}");
            var shift = token.CloseIndex - token.OpenIndex + 1;
            return new TagInsertion(insertion, shift);
        }
    }
}