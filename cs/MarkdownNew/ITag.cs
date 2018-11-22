using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    interface ITag
    {
        bool IsValidOpenTagFromPosition(string someString, int position);
        bool IsValidCloseTagFromPosition(string someString, int position);
    }
}
