using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace Markdown
{
    public class Md
    {
        private readonly IMdTag[] orderedTags;
        private readonly Stack<HtmlTextWriterTag> openTags = new Stack<HtmlTextWriterTag>();
        private string renderingString;
        
        public Md(IMdTag[] orderedTags)
        {
            this.orderedTags = orderedTags;
        }

        public Md() =>
            new Md(new []
            {
                new MdWrappingTag("__", HtmlTextWriterTag.Strong,this),
                new MdWrappingTag("_", HtmlTextWriterTag.U,this),
            });

        public IEnumerable<HtmlTextWriterTag> OpenTags => openTags;
        
        public string Render(string markdowned)
        {
            throw new NotImplementedException();
        }
    }
}