using System.Collections.Generic;

namespace Markdown
{
    public interface IMarkdownProcessor
    {
        public List<TokenMd> FormatTokens(List<TokenMd> tokens);
    }
}