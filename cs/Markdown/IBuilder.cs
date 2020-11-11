using System.Collections.Generic;

namespace Markdown
{
    public interface IBuilder
    {
        public string Build(List<TextToken> textTokens);
    }
}