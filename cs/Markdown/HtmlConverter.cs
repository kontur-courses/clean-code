using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private readonly Dictionary<TokenType, ITokenConverter> mapping;

        public HtmlConverter(Dictionary<TokenType, ITokenConverter> mapping)
        {
            this.mapping = mapping;
        }

        public string ConvertTokens(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var converter = mapping[token.Type];

                if (token.CanHaveChildTokens && token.ChildTokens.Count != 0)
                {
                    var newValue = ConvertTokens(token.ChildTokens);
                    var newToken = CreateNewToken(token, newValue);

                    result.Append(converter.Convert(newToken));
                    continue;
                }

                result.Append(converter.Convert(token));
            }

            return result.ToString();
        }

        private static Token CreateNewToken(Token token, string value)
        {
            var arguments = new[] {typeof(int), typeof(string), typeof(int)};
            var constructor = token.GetType().GetConstructor(arguments);
            var parameters = new object[] {token.Position, value, token.EndPosition};

            return (Token) constructor!.Invoke(parameters);
        }
    }
}