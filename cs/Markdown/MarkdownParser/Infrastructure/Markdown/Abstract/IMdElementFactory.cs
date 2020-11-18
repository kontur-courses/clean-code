using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    /// <summary>
    /// Basicly you don't need to inherit from this interface directly,
    /// look at <see cref="MdElementFactory{TElem,TToken}"/>
    /// </summary>
    public interface IMdElementFactory
    {
        Type TokenType { get; }
        bool CanCreate(Token token);
        MarkdownElement Create(Token token, Token[] nextTokens);
    }

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

    public abstract class MdPairElementFactory<TElem, TOpening, TClosing> :
        MdElementFactory<TElem, TokenPair>
        where TOpening : PairedToken
        where TClosing : PairedToken
        where TElem : MarkdownElement
    {
        public sealed override Type TokenType { get; } = typeof(TOpening);
        public Type ClosingType { get; } = typeof(TClosing);

        public sealed override bool CanCreate(TokenPair token) =>
            base.CanCreate(token) &&
            token.Opening is TOpening opening &&
            token.Closing is TClosing closing &&
            CanCreate(opening, token.Inner, closing);

        public sealed override TElem Create(TokenPair token, Token[] _) =>
            Create((TOpening) token.Opening, token.Inner, (TClosing) token.Closing);

        protected abstract TElem Create(TOpening opening, Token[] innerTokens, TClosing closing);
        protected virtual bool CanCreate(TOpening opening, Token[] inner, TClosing closing) => true;
    }
}