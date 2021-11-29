using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public class TokensToHtmlConverter : IConverter<IEnumerable<Token>, string>
    {
        public string Convert(IEnumerable<Token> tokens)
        {
            var html = new StringBuilder();
            foreach (var token in tokens)
            {
                html.Append(token.Type is TokenType.Word or TokenType.EscapeSymbol
                    ? token.Value
                    : GetHtmlTagRepresentation(token.Type));
            }

            return html.ToString();
        }

        private static string GetHtmlTagRepresentation(TokenType type)
        {
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            return type switch
            {
                TokenType.BoldOpened => "<strong>",
                TokenType.ItalicOpened => "<em>",
                TokenType.HeaderOpened => "<h1>",
                TokenType.BoldClosed => "</strong>",
                TokenType.ItalicClosed => "</em>",
                TokenType.HeaderClosed => "</h1>",
                _ => throw new ArgumentException("Unexpected type")
            };
        }
    }
}