using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenEscaper
    {
        private readonly List<char> symbolsToEscape;

        public TokenEscaper(IReadOnlyCollection<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            symbolsToEscape = tokens
                .Select(token => token.Pattern.StartTag[0])
                .Concat(new[] {'\\'})
                .ToList();
        }

        public EscapedText EscapeTokens(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var escapedTextBuilder = new EscapedTextBuilder(new Context(text));

            foreach (var context in escapedTextBuilder)
            {
                if (IsEscapeSymbol(context))
                    escapedTextBuilder.SkipNextSymbol();
                else
                    escapedTextBuilder.Append(context.CurrentSymbol);
            }

            return escapedTextBuilder.Build();
        }

        public bool IsEscapeSymbol(Context context) =>
            context.Index < context.Text.Length - 1
            && context.CurrentSymbol == '\\'
            && symbolsToEscape.Contains(context.Skip(1).CurrentSymbol);
    }
}