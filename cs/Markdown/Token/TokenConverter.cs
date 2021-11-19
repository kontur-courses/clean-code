using System.Collections.Generic;
using System.Linq;

namespace Markdown.Token
{
    public class TokenConverter
    {
        private readonly Stack<Token> tokens;

        public TokenConverter()
        {
            tokens = new Stack<Token>();
        }

        public IEnumerable<Token> GetTokens() => tokens; 
        
        public TokenConverter FindTokens(string source)
        {
            GetBuildsMachine(source)
                .Run();
            return this;
        }

        private Machine GetBuildsMachine(string source)
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
                    var currentToken = tokens.Last();
                    currentToken.Length = i - currentToken.StartPosition + 1;
                    if (currentToken.Tag.ClosingMarkup != s[i].ToString())
                        tokens.Pop();
                });

            return Machine.CreateMachine(source, defaultState);
        }
    }
}