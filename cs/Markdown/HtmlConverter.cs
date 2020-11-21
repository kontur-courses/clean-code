using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private readonly Dictionary<TokenType, ITagTokenConverter> mapper = new Dictionary<TokenType, ITagTokenConverter>
        {
            {TokenType.Emphasized, new EmphasizedTagTokenConverter()},
            {TokenType.Heading, new HeadingTagTokenConverter()},
            {TokenType.Strong, new StrongTagTokenConverter()},
            {TokenType.PlainText, new PlainTextTagTokenConverter()},
            {TokenType.Image, new ImageTagTokenConverter()}
        };

        public void AddMapping(TokenType type, ITagTokenConverter tagTokenConverter)
        {
            mapper.Add(type, tagTokenConverter);
        }

        public string ConvertTokens(List<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var tagToken = mapper[token.Type];

                if (token.ChildTokens.Count != 0)
                {
                    var newValue= ConvertTokens(token.ChildTokens);
                    token.Value = newValue;
                }

                result.Append(tagToken.Convert(token));
            }

            return result.ToString();
        }
    }
}