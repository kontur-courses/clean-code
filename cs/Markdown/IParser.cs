using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface IParser
    {
        public string ParseMdToHTML(string markDownText);
    }
}
