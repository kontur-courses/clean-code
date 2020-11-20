using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public abstract class MdPairElementFactory<TElem, TOpening, TClosing> :
        MdElementFactory<TElem, TokenPair>
        where TOpening : PairedToken
        where TClosing : PairedToken
        where TElem : MarkdownElement
    {
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