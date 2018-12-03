using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
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
