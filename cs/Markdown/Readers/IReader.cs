using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Readers
{
    interface IReader
    {
        IToken ReadToken(string text, int position);
    }
}
