using System;

namespace Markdown
{
    public class MdTokenReader : TokenReader
    {
        public MdTokenReader(string text) : base(text)
        {
            AddBasicToken<MdHeaderToken>("\n# ", "\n");
            AddBasicToken<MdBoldToken>(" __", "__ ");
            AddBasicToken<MdItalicToken>(" _", "_ ");
        }
    }
}