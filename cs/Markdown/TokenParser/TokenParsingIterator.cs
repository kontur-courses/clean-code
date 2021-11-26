using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tags;
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
        private readonly LinkParsingIteratorState linkState;

        public TokenParsingIterator(IEnumerator<Token> tokens)
        {
            enumerator = new BufferedEnumerator<Token>(tokens);
            cursiveState = new CursiveParsingIteratorState(this);
            boldState = new BoldParsingIteratorState(this);
            escapeState = new EscapeParsingIteratorState(this);
            newLineState = new NewLineParsingIteratorState(this);
            header1State = new Header1ParsingIteratorState(this);
            textState = new TextParsingIteratorState(this);
            linkState = new LinkParsingIteratorState(this);
        }

        public Token Current => enumerator.Current;

        public IEnumerable<TagNode> Parse()
        {
            while (TryMoveNext(out var current))
            {
                var node = ParseToken(current);
                if (contexts.TryPeek(out var context))
                    context.AddChild(node);
                else
                    yield return node;
            }

            if (TryFlushContextsUntil(out var contextNode, TokenContext.IsHeader1))
                yield return ToNode(contextNode);
        }


        public bool TryFlushContextsUntil(out TokenContext resultContext, Func<TokenContext, bool> isStopper)
        {
            if (isStopper == null) throw new ArgumentNullException(nameof(isStopper));
            var stack = new Stack<string>();
            while (contexts.TryPop(out var context))
            {
                if (isStopper(context) || contexts.Count == 0)
                {
                    var node = Token.Text(StringUtils.Join(stack)).ToNode();
                    context.AddChild(node);
                    resultContext = context;
                    return true;
                }

                stack.Push(context.ToText());
            }

            resultContext = default;
            return false;
        }

        public TagNode ParseToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Escape => escapeState.Parse(),
                TokenType.Cursive => cursiveState.Parse(),
                TokenType.Bold => boldState.Parse(),
                TokenType.Header1 => header1State.Parse(),
                TokenType.NewLine => newLineState.Parse(),
                TokenType.OpenSquareBracket or TokenType.CloseSquareBracket => linkState.Parse(),
                _ => textState.Parse()
            };
        }

        public bool TryPopContext(out TokenContext context) => contexts.TryPop(out context);

        public void PushContext(TokenContext tokenContext) => contexts.Push(tokenContext);

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

        public TagNode ToNode(TokenContext context)
        {
            if (context.Token.Type == TokenType.Header1 && contexts.Count == 0)
                return new TagNode(Token.Header1.ToTag(), context.Children.ToArray());
            return Tag.Text(context.ToText()).ToNode();
        }

        public bool AnyContext(Func<TokenContext, bool> predicate) => contexts.Any(predicate);
    }
}