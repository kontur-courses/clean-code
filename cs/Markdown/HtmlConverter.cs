using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private readonly Dictionary<TokenType, ITagToken> mapper = new Dictionary<TokenType, ITagToken>
        {
            {TokenType.Emphasized, new EmphasizedTagToken()},
            {TokenType.Heading, new HeadingTagToken()},
            {TokenType.Strong, new StrongTagToken()},
            {TokenType.PlainText, new PlainTextTagToken()},
            {TokenType.Image, new ImageTagToken()}
        };

        public void AddMapping(TokenType type, ITagToken tagToken)
        {
            mapper.Add(type, tagToken);
        }

        public string ConvertTokens(List<Token> tokens)
        {
            var stack = new Stack<Token>();
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var tagToken = mapper[token.Type];

                if (stack.Count != 0 && token.IsInsideToken(stack.Last()))
                {
                    if (stack.Last().Type == TokenType.Emphasized && token.Type == TokenType.Strong)
                        continue;

                    result.Replace(token.Value, tagToken.Convert(token));
                    continue;
                }

                result.Append(tagToken.Convert(token));
                stack.Push(token);
            }

            return result.ToString();
        }
    }
}