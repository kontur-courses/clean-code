using System.Collections.Generic;

namespace Markdown.ASTNodes.StyleElement
{
    public class BoldStyleElement : IStyleElement
    {
        public BoldStyleElement(Token[] innerTokens)
        {
            InnerTokens = innerTokens;
        }

        public IEnumerable<Token> InnerTokens { get; set; }

        public static string DoubleKeyword => "__";

        public static TokenType TokenType => TokenType.BoldDelimiter;

        public IElement Child { get; set; }

        public string TransformToHtml(string value) => $"<strong>{value}</strong>";

        public string GetRawValue(string value) => $"{DoubleKeyword}{value}{DoubleKeyword}";
    }
}
