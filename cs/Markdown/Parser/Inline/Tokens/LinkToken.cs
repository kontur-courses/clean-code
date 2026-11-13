using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public class LinkToken : Token
    {
        public string Text { get; }
        public string Url { get; }
        public LinkToken(string text, string url, int position) : base(position)
        {
            Text = text;
            Url = url;
        }
    }
}
