using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenConverter
    {
        private readonly Stack<Token> tokens;
        private string source;

        public TokenConverter()
        {
            tokens = new Stack<Token>();
            source = "";
        }

        public IEnumerable<Token> GetTokens() => tokens;

        public string Build()
        {
            var builder = source.Select(symbol => symbol.ToString()).ToList();
            foreach (var token in tokens)
            {
                builder[token.StartPosition] = token.Tag.OpeningTag;
                builder[token.StartPosition + token.Length - 1] = token.Tag.ClosingTag;
            }
            return string.Join("", builder);
        }

        public TokenConverter SetMarkupString(string markup)
        {
            source = markup;
            return this;
        }

        public TokenConverter FindTokens()
        {
            GetBuildsMachine(source)
                .Run();
            return this;
        }

        private Machine GetBuildsMachine(string input)
        {
            var italicsTag = new ItalicsTag();

            var defaultState = State.CreateState();
            var italicsState = State.CreateState();

            defaultState.AddTransition(italicsTag.OpeningMarkup[0], italicsState)
                .SetFallback(defaultState);

            italicsState
                .AddTransition(italicsTag.ClosingMarkup[0], defaultState)
                .SetFallback(italicsState)
                .SetOnEntry((s, i) => { tokens.Push(new Token(i, new ItalicsTag())); })
                .SetOnExit((s, i) =>
                {
                    var currentToken = tokens.Peek();
                    currentToken.Length = i - currentToken.StartPosition + 1;
                    if (currentToken.Tag.ClosingMarkup != s[i].ToString() ||
                        currentToken.Tag.IsBrokenMarkup(s, currentToken.StartPosition, currentToken.Length))
                        tokens.Pop();
                });

            return Machine.CreateMachine(input, defaultState);
        }
    }
}