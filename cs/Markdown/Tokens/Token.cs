using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    class Token : IToken
    {
        public string Text { get; }

        public Token(string text)
        {
            Text = text;
        }

        public string GetContent()
        {
            return Text;
        }
    }
}
