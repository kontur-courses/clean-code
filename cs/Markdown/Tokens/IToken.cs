using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    interface IToken
    {
        string Text { get; }
        string GetContent();
    }
}
