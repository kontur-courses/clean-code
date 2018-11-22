using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    interface IMarkdownRenderer
    {
        string ConvertFromMarkdownToHtml(string markdown);
    }
}
