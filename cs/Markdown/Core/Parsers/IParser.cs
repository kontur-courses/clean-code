using System.Collections.Generic;

namespace Markdown.Core.Parsers
{
    public interface IParser
    {
        List<TagToken> ParseLine(string line);
    }
}