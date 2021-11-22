using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Models
{
    public class TokenEscaper
    {
        private readonly string text;
        private readonly List<char> symbolsToEscape;

        private readonly StringBuilder builder = new();
        private readonly List<int> escapedSymbolsBefore = new();
        private int escapedCount;

        public TokenEscaper(string text, IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentException($"{nameof(tokens)} can't be null.", nameof(tokens));

            this.text = text ?? throw new ArgumentException($"{nameof(text)} can't be null.", nameof(text));
            symbolsToEscape = tokens
                .Select(token => token.Pattern.StartTag.FirstOrDefault())
                .Concat(new[] {'\\'})
                .ToList();
        }

        public EscapedText EscapeTokens()
        {
            for (var i = 0; i < text.Length; i++)
                AppendSymbol(ref i);

            return new EscapedText(builder.ToString(), escapedSymbolsBefore);
        }

        private void AppendSymbol(ref int i)
        {
            if (IsEscapeSymbol(i))
            {
                escapedCount += 2;
                i++;
            }
            else
            {
                builder.Append(text[i]);
                escapedSymbolsBefore.Add(escapedCount);
            }
        }

        public bool IsEscapeSymbol(int index) =>
            index < text.Length - 1
            && text[index] == '\\'
            && symbolsToEscape.Contains(text[index + 1]);
    }
}