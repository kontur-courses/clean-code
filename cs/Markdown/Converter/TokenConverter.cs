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
                    builder.Append(startToken.Tag.GetOpeningHtmlTag());
                    i += startToken.Tag.MdTag.Length - 1;
                    continue;
                }

                var endPairToken = tokens.FirstOrDefault(token => token.EndPosition == i && token.Tag.IsPairMdTag);

                if (endPairToken != null)
                {
                    if (endPairToken.Tag.MdTag.Length > 1 && endPairToken.Tag.IsPairMdTag)
                        builder.Remove(builder.Length - 1, 1);
                    builder.Append(endPairToken.Tag.GetClosingHtmlTag());
                }

                var endNonPairToken = tokens.FirstOrDefault(token => token.EndPosition == i && !token.Tag.IsPairMdTag);

                if (endNonPairToken != null)
                {
                    if (endPairToken == null)
                    {
                        builder.Append(text[i]);
                        builder.Append(endNonPairToken.Tag.GetClosingHtmlTag());
                    }
                    else
                        builder.Append(endNonPairToken.Tag.GetClosingHtmlTag());
                    
                }
                
                if (endPairToken != null || endNonPairToken != null)
                    continue;

                builder.Append(text[i]);
            }

            return builder.ToString();
        }
    }
}