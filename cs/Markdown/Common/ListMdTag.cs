using System.Collections.Generic;
using System.Linq;

namespace Markdown.Common
{
    public class ListMdTag : BlockMdTag
    {
        protected string HtmlOpenListTag { get; }
        protected string HtmlCloseListTag { get; }

        public ListMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag, string htmlOpenListTag,
            string htmlCloseListTag)
            : base(mdTag, htmlOpenTag, htmlCloseTag)
        {
            HtmlOpenListTag = htmlOpenListTag;
            HtmlCloseListTag = htmlCloseListTag;
            IsMultiLine = true;
        }

        public override bool TryGetToken(string text, Tag openTag, IList<Tag> closeTags, out Token token,
            out Tag closeTag)
        {
            if (base.TryGetToken(text, openTag, closeTags, out var listToken, out var noTag))
            {
                listToken.AddToken(new Token(listToken.Value, listToken.Position, new BlockMdTag(MdTag, HtmlOpenTag, HtmlCloseTag)));
                closeTag = null;
                token = listToken;
                return true;
            }

            closeTag = null;
            token = null;
            return false;
        }

        public Token RebuildListToken(Token list, Token childList)
        {
            var newListToken = new Token(list.Value + childList.Value, list.Position, this);
            foreach (var child in list.ChildTokens.Concat(childList.ChildTokens))
                newListToken.AddToken(child);
            return newListToken;
        }
    }
}