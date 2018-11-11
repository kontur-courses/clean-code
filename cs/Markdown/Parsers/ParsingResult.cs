using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Elements;

namespace Markdown.Parsers
{
    class ParsingResult
    {
        public readonly bool Success;
        public readonly IMarkdownElement Element;

        public ParsingResult(bool success, IMarkdownElement element)
        {
            Success = success;
            Element = element;
        }
    }
}
