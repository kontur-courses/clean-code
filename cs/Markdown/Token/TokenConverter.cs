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
            var builder = source.Select(symbol => symbol.ToString()).ToList();
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
            source = markup;
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
            var anyTokens = new Stack<Token>();
            
            var startState = State.CreateState();
            var markdownState = State.CreateState();
            var italicsWaitingState = State.CreateState();
            var italicsEndState = State.CreateState();
            var boldWaitingState = State.CreateState();
            var boldPreEndState = State.CreateState();
            var boldInnerItalicsWaitingState = State.CreateState();
            var boldInnerItalicsEndState = State.CreateState();
            var boldAfterInnerItalicsState = State.CreateState();
            var boldEndState = State.CreateState();

            startState.AddTransition('_', markdownState)
                .SetFallback(startState);

            markdownState.AddTransition('_', boldWaitingState)
                .SetFallback(italicsWaitingState);

            italicsWaitingState.AddTransition('_', italicsEndState)
                .SetFallback(italicsWaitingState)
                .SetOnEntry((s, i) => { anyTokens.Push(new Token(i - 1, new ItalicsTag())); });

            italicsEndState.AddTransition('_', markdownState)
                .SetFallback(startState)
                .SetOnEntry((s, i) =>
                {
                    var token = anyTokens.Peek();
                    token.Length = i - token.StartPosition + 1;
                });

            boldWaitingState.AddTransition('_', boldPreEndState)
                .SetFallback(boldWaitingState)
                .SetOnEntry((s, i) => { anyTokens.Push(new Token(i - 1, new BoldTag())); });

            boldPreEndState.AddTransition('_', boldEndState)
                .SetFallback(boldInnerItalicsWaitingState);

            boldInnerItalicsWaitingState.AddTransition('_', boldInnerItalicsEndState)
                .SetFallback(boldInnerItalicsWaitingState)
                .SetOnEntry((s, i) => { anyTokens.Push(new Token(i - 1, new ItalicsTag())); });

            boldInnerItalicsEndState.AddTransition('_', boldPreEndState)
                .SetFallback(boldAfterInnerItalicsState)
                .SetOnEntry((s, i) =>
                {
                    var token = anyTokens.Pop();
                    token.Length = i - token.StartPosition + 1;
                    var upperToken = anyTokens.Pop();
                    anyTokens.Push(token);
                    anyTokens.Push(upperToken);
                });
            
            boldAfterInnerItalicsState.AddTransition('_', boldPreEndState)
                .SetFallback(boldAfterInnerItalicsState);

            boldEndState.AddTransition('_', markdownState)
                .SetFallback(startState)
                .SetOnEntry((s, i) =>
                {
                    var token = anyTokens.Peek();
                    token.Length = i - token.StartPosition + 1;
                });

            tokens = GetNotBrokenTokens(anyTokens);
            
            return Machine.CreateMachine(startState);
        }

        private IEnumerable<Token> GetNotBrokenTokens(IEnumerable<Token> anyTokens)
        {
            return anyTokens.Where(t => !t.Tag.IsBrokenMarkup(source, t.StartPosition, t.Length));
        }
    }
}