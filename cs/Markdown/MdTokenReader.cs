using System;

namespace Markdown
{
    public class MdTokenReader : TokenReader
    {
        public MdTokenReader(string text) : base(text)
        {
            AddToken((reader, parent)
                => ReadFullLineToken<MdHeaderToken>(reader, parent, "# ", ""));
            AddToken((reader, parent)
                => ReadHighlightToken<MdBoldToken>(reader, parent, "__", "__"));
            AddToken((reader, parent)
                => ReadHighlightToken<MdItalicToken>(reader, parent, "_", "_"));
        }
        
        public TToken ReadFullLineToken<TToken>(TokenReader reader, Token parent, string startWith, string endWith)
        {
            throw new NotImplementedException();
        }

        public TToken ReadHighlightToken<TToken>(TokenReader reader, Token parent, string startWith, string endWith)
        {
            throw new NotImplementedException();
        }
    }
}