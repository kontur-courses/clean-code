using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenizer
    {
        TokenText ParseText(string sourceText);
    }
}