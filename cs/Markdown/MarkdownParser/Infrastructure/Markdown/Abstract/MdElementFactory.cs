using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public abstract class MdElementFactory<TElem, TToken> : IMdElementFactory
        where TToken : Token
        where TElem : MarkdownElement
    {
        public virtual Type TokenType { get; } = typeof(TToken);

        public virtual bool CanCreate(TToken token) => true;
        public abstract TElem Create(TToken token, Token[] nextTokens);

        bool IMdElementFactory.CanCreate(Token token) => token is TToken t && CanCreate(t);
        MarkdownElement IMdElementFactory.Create(Token token, Token[] nextTokens) => Create((TToken) token, nextTokens);
    }
}