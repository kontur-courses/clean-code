using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenConverter
    {
        private IEnumerable<Token> tokens;
        private string source;

        public TokenConverter()
        {
            tokens = new Stack<Token>();
            source = "";
        }

        public IEnumerable<Token> GetTokens() => tokens;

        public string Build()
        {
            var builder = source.Take(source.Length - 1)
                .Select(symbol => symbol.ToString()).ToList();
            foreach (var token in tokens)
            {
                builder[token.StartPosition] = token.Tag.OpeningTag;
                for (var i = 1; i < token.Tag.OpeningMarkup.Length; i++)
                    builder[token.StartPosition + i] = "";
                builder[token.StartPosition + token.Length - 1] = token.Tag.ClosingTag;
                for (var i = 1; i < token.Tag.ClosingMarkup.Length; i++)
                    builder[token.StartPosition + token.Length - i - 1] = "";
            }

            return string.Join("", builder);
        }

        public TokenConverter SetMarkupString(string markup)
        {
            source = markup + ' ';
            return this;
        }

        public TokenConverter FindTokens()
        {
            GetBuildsMachine()
                .Run(source);
            return this;
        }

        private Machine GetBuildsMachine()
        {
            var italicsTokens = new Stack<Token>();
            var boldTokens = new Stack<Token>();
            var shieldingTokens = new Stack<Token>();

            var startState = State.CreateState();
            var shieldingState = State.CreateState();
            var italicsState = State.CreateState();
            var boldState = State.CreateState();

            startState
                .AddTransition('\\', shieldingState)
                .AddTransition('_', italicsState)
                .SetFallback(startState);

            shieldingState
                .SetFallback(startState)
                .SetOnEntry((s, i) =>
                {
                    if (s[i + 1] == '_' || s[i + 1] == '\\')
                        shieldingTokens.Push(new Token(i, new ShieldingTag()) {Length = 1});
                });

            italicsState
                .AddTransition('_', boldState)
                .AddTransition('\\', shieldingState)
                .SetFallback(startState)
                .SetOnEntry((s, i) =>
                {
                    if (s[i + 1] == '_')
                        return;
                    if (!italicsTokens.TryGetPeekItem(out var italicsToken))
                        italicsTokens.Push(new Token(i, new ItalicsTag()));
                    else if (italicsToken.Length != 0)
                        italicsTokens.Push(new Token(i, new ItalicsTag()));
                    else if (boldTokens.TryGetPeekItem(out var boldToken)
                             && boldToken.StartPosition > italicsToken.StartPosition)
                        return;
                    else
                        italicsToken.Length = i + 1 - italicsToken.StartPosition;
                });

            boldState
                .AddTransition('_', italicsState)
                .AddTransition('\\', shieldingState)
                .SetFallback(startState)
                .SetOnEntry((s, i) =>
                {
                    if (!boldTokens.TryGetPeekItem(out var boldToken))
                        boldTokens.Push(new Token(i - 1, new BoldTag()));
                    else if (boldToken.Length != 0)
                        boldTokens.Push(new Token(i - 1, new BoldTag()));
                    else if (italicsTokens.TryGetPeekItem(out var italicsToken)
                             && italicsToken.Length == 0
                             && boldToken.StartPosition > italicsToken.StartPosition)
                        boldTokens.Pop();
                    else
                        boldToken.Length = i - boldToken.StartPosition + 1;
                });

            tokens = GetNotBrokenTokens(Concat(italicsTokens, boldTokens, shieldingTokens));

            return Machine.CreateMachine(startState);
        }

        private IEnumerable<Token> Concat(params IEnumerable<Token>[] collections)
        {
            return collections.SelectMany(collection => collection);
        }

        private IEnumerable<Token> GetNotBrokenTokens(IEnumerable<Token> anyTokens)
        {
            return anyTokens.Where(t => !t.Tag.IsBrokenMarkup(source, t.StartPosition, t.Length));
        }
    }
}