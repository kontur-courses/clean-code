using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var reader = new TokenReader();
            var tokens = reader.ReadTokens(text);
            return MdConvert.ToHtml(tokens);
        }
    }
}
