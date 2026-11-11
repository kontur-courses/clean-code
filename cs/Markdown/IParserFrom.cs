using System.Collections.Generic;

namespace Markdown;

public interface IParserFrom
{
    List<MarkupToken> Parse(string text);
}
