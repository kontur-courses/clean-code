using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers.Tags
{
    public interface ITag
    {
        public ITag ToText();
        public ITag ToHtml();
        public ITag ToMarkdown();
    }
}
