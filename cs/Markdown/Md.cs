using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var tokens = new Tokenizer().GetTokens(text, new List<string>{"_", "__"});
            throw new NotImplementedException();
        }
    }
}
