using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.TokenParser.TokenParsingIteratorState;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenParsingIterator
    {
        private readonly BufferedEnumerator<Token> enumerator;
        private readonly Stack<TokenContext> contexts = new();
        private readonly CursiveParsingIteratorState cursiveState;
        private readonly BoldParsingIteratorState boldState;
        private readonly EscapeParsingIteratorState escapeState;
        private readonly NewLineParsingIteratorState newLineState;
        private readonly Header1ParsingIteratorState header1State;
        private readonly TextParsingIteratorState textState;

        public TokenParsingIterator(IEnumerator<Token> tokens)
        {
            enumerator = new BufferedEnumerator<Token>(tokens);
            cursiveState = new CursiveParsingIteratorState(this);
            boldState = new BoldParsingIteratorState(this);
            escapeState = new EscapeParsingIteratorState(this);
            newLineState = new NewLineParsingIteratorState(this);
            header1State = new Header1ParsingIteratorState(this);
            textState = new TextParsingIteratorState(this);
        }

        public Token Current => enumerator.Current;

        public IEnumerable<TokenNode> Parse()
        {
            while (TryMoveNext(out var current))
            {
                var node = ParseToken(current);
                if (contexts.TryPeek(out var context))
                    context.Children.Add(node);
                else
                    yield return node;
            }

            if (TryFlushContexts(out var contextNode)) yield return contextNode;
        }

        public bool TryFlushContexts(out TokenNode node)
        {
            var stack = new Stack<string>();
            while (contexts.TryPop(out var context))
                if (context.Token.Type != TokenType.Header1)
                {
                    stack.Push(context.ToText());
                    if (contexts.Count == 0)
                    {
                        node = FlushToTextToken(stack, context);
                        return true;
                    }
                }
                else
                {
                    node = FlushToTextToken(stack, context);
                    return true;
                }

            node = default;
            return false;
        }

        public TokenNode ParseToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Escape => escapeState.Parse(),
                TokenType.Cursive => cursiveState.Parse(),
                TokenType.Bold => boldState.Parse(),
                TokenType.Header1 => header1State.Parse(),
                TokenType.NewLine => newLineState.Parse(),
                _ => textState.Parse()
            };
        }

        public bool TryPeekContext(out TokenContext context) => contexts.TryPeek(out context);

        public void PushContext(TokenContext tokenContext) => contexts.Push(tokenContext);

        public TokenContext PopContext() => contexts.Pop();

        public bool TryGetPreviousToken(out Token token) => enumerator.TryGetPrevious(out token);

        public bool TryGetNextToken(out Token token) => enumerator.TryGetNext(out token);

        public bool TryMoveNext(out Token token)
        {
            if (enumerator.MoveNext())
            {
                token = enumerator.Current;
                return true;
            }

            token = default;
            return false;
        }

        public void PushToBuffer(Token token) => enumerator.PushToBuffer(token);

        private TokenNode FlushToTextToken(IEnumerable<string> stack, TokenContext context)
        {
            contexts.Clear();
            var node = Token.Text(StringUtils.Join(stack)).ToNode();
            return context.Token.Type switch
            {
                TokenType.Header1 => new TokenNode(Token.Header1, context.Children.Append(node).ToArray()),
                _ => node
            };
        }
    }
}