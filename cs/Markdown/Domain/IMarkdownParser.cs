using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Domain
{
    public interface IMarkdownParser
    {
        List<Token> ParseMarkdown(string line);
    }
}
