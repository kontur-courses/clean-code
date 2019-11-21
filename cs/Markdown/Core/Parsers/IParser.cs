using System.Collections.Generic;

namespace Markdown.Core.Parsers
{
    interface IParser
    {
        List<TagToken> ParseLine(string line);
    }
}