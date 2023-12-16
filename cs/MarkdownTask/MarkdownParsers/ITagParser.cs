using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public interface ITagParser
    {
        ICollection<Token> ParseTag(string markdown);
    }
}
