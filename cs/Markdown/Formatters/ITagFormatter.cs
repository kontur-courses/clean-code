using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface ITagFormatter
    {
        string OnOpen(string tag);
        string OnClose(string tag);
    }
}
