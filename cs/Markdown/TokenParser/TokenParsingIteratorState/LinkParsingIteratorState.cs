using System;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class LinkParsingIteratorState : TokenParsingIteratorState
    {
        public LinkParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse()
        {
            var token = Iterator.Current;
            return token.Type switch
            {
                TokenType.OpenSquareBracket => ParseOpenSquareBracket(),
                TokenType.CloseSquareBracket => ParseCloseSquareBracket(),
                _ => Tag.Text(token.Value).ToNode()
            };
        }

        private TagNode ParseCloseSquareBracket()
        {
            if (Iterator.AnyContext(TokenContext.IsLink))
            {
                if (Iterator.TryFlushContextsUntil(out var context, TokenContext.IsLink))
                {
                    return ParseLinkAddress(context);
                }

                throw new Exception("Link context was not found");
            }

            return Token.CloseSquareBracket.ToText().ToNode();
        }

        private TagNode ParseLinkAddress(TokenContext linkContext)
        {
            if (Iterator.TryMoveNext(out var token))
            {
                if (token.Type == TokenType.OpenCircleBracket)
                {
                    return ParseLinkAddress2(linkContext);
                }

                Iterator.PushToBuffer(token);
            }

            linkContext.AddChild(Token.CloseSquareBracket.ToNode());
            return Tag.Text(linkContext.ToText()).ToNode();
        }

        private TagNode ParseLinkAddress2(TokenContext linkContext)
        {
            var sb = new StringBuilder();
            while (Iterator.TryMoveNext(out var next))
            {
                switch (next)
                {
                    case { Type: TokenType.CloseCircleBracket }:
                        return new TagNode(Tag.Link(sb.ToString()), linkContext.Children.ToArray());
                    case { Type: TokenType.Escape }:
                        sb.Append(ParseEscapeInLinkAddress());
                        break;
                    default: 
                        sb.Append(next.Value);
                        break;
                }
            }

            linkContext.AddChild(Token.CloseSquareBracket.ToNode());
            linkContext.AddChild(Token.OpenCircleBracket.ToNode());
            return Tag.Text($"{linkContext.ToText()}{sb}").ToNode();
        }

        private string ParseEscapeInLinkAddress()
        {
            if (Iterator.TryMoveNext(out var next) && next.Type == TokenType.CloseCircleBracket)
                return Token.CloseCircleBracket.Value;

            return $"{Token.Escape.Value}{Token.CloseCircleBracket.Value}";
        }

        private TagNode ParseOpenSquareBracket()
        {
            if (Iterator.AnyContext(TokenContext.IsLink))
            {
                if (Iterator.TryFlushContextsUntil(out var node, TokenContext.IsLink))
                {
                    Iterator.PushToBuffer(Token.OpenSquareBracket);
                    return Iterator.ToNode(node);
                }

                throw new Exception("Link context was not found");
            }

            if (Iterator.TryMoveNext(out var token))
            {
                Iterator.PushContext(new TokenContext(Token.OpenSquareBracket));
                return Iterator.ParseToken(token);
            }

            return Token.OpenSquareBracket.ToNode();
        }
    }
}