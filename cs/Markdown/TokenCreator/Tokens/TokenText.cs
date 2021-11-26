using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public sealed class TokenText : IToken
    {
        public static TokenText FromText(string value) => new() { Value = value };
        
        private readonly HashSet<string> markupSymbols = new() { "\n", "_", "__", "\\" };
        
        public TokenType TokenType => TokenType.Text;
        public string Value { get; private init; }

        public bool CanParse(string symbol) => true;

        public IToken Create(string[] text, int index)
        {
            var innerText = new StringBuilder();
            innerText.Append(text[index]);
            var substringIndex = index + 1;
            while (substringIndex < text.Length && !markupSymbols.Contains(text[substringIndex]))
            {
                innerText.Append(text[substringIndex]);
                substringIndex++;
            }

            return new TokenText { Value = innerText.ToString() };
        }
    }
}