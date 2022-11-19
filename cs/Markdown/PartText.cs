using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Markdown
{
    public class PartText
    {
        public PartText(string text, Tag tag)
        {
            Text = text;
            Tag = tag;
        }
        public PartText(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
        public Tag Tag { get; set; }
    }

}
