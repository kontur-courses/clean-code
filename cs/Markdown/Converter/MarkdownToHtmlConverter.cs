using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Converter
{
    public class MarkdownToHtmlConverter : Converter
    {
        public MarkdownToHtmlConverter() : base(new MdTagStorage(), new HtmlTagStorage())
        {
        }
    }
}
