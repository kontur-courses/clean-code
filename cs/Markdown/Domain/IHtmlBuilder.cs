using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Domain
{
    public interface IHtmlBuilder
    {
        string BuildHtmlFromMarkdown(string markdownText, List<Token> tokens);
    }
}
