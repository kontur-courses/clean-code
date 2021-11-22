using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Converter
{
    public class TokenConverter : IConverter
    {
        private readonly IReadOnlyCollection<Tag> availableTags;
        private const char EscapeChar = '\\';

        public TokenConverter(IReadOnlyCollection<Tag> availableTags)
        {
            this.availableTags = availableTags;
        }
        
        public string ConvertTokensInText(List<IToken> tokens, string text)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == EscapeChar && i + 1 < text.Length)
                {
                    if (text[i + 1] == EscapeChar)
                    {
                        builder.Append(text[i]);
                        i++;
                        continue;
                    }

                    if (availableTags.Any(tag => tag.MdTag.StartsWith(text[i +  1].ToString())))
                    {
                        continue;
                    }
                }
                
                var startToken = tokens.FirstOrDefault(token => token.StartPosition == i);

                if (startToken != null)
                {
                    builder.Append(startToken.TagType.GetOpeningHtmlTag());
                    i += startToken.TagType.MdTag.Length - 1;
                    continue;
                }
                
                if (IsEndOfToken(tokens, text, i, builder)) continue;

                builder.Append(text[i]);
            }

            return builder.ToString();
        }

        private static bool IsEndOfToken(List<IToken> tokens, string text, int i, StringBuilder builder)
        {
            var endingTokens = tokens.Where(token => token.EndPosition == i).Reverse().ToList();

            if (endingTokens.Any())
            {
                foreach (var token in endingTokens)
                {
                    if (endingTokens.Count == 1 && !token.TagType.IsPairMdTag)
                    {
                        builder.Append(text[i]);
                        builder.Append(token.TagType.GetClosingHtmlTag());
                        continue;
                    }

                    if (token.TagType.MdTag.Length > 1 && token.TagType.IsPairMdTag)
                        builder.Remove(builder.Length - 1, 1);
                    builder.Append(token.TagType.GetClosingHtmlTag());
                }

                return true;
            }

            return false;
        }
    }
}