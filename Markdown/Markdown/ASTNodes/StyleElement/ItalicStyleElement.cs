using System.Collections.Generic;

namespace Markdown.ASTNodes.StyleElement
{
    public class ItalicStyleElement : IStyleElement
    {
        public ItalicStyleElement(Token[] innerTokens)
        {
            InnerTokens = innerTokens;
        }


        public IEnumerable<Token> InnerTokens { get; set; }

        public static string DoubleKeyword => "_";

        public static TokenType TokenType => TokenType.ItalicDelimiter;

        public IElement Child { get; set; }

        public string TransformToHtml(string value) => $"<em>{value}</em>";

        public string GetRawValue(string value) => $"{DoubleKeyword}{value}{DoubleKeyword}";
    }
}
