using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public abstract class MdElementFactory<TElem, TToken> : IMdElementFactory
        where TToken : Token
        where TElem : MarkdownElement
    {
        public virtual bool CanCreate(TToken token) => true;
        public abstract TElem Create(TToken token, Token[] nextTokens);

        public bool CanCreate(MarkdownElementContext context) =>
            context.CurrentToken is TToken t && CanCreate(t);

        public MarkdownElement Create(MarkdownElementContext context) =>
            Create((TToken) context.CurrentToken, context.NextTokens);
    }
}