using System.Collections.Generic;

namespace Markdown
{
    public abstract class Parser
    {
        public abstract List<Token> GetTokens(string text);
    }
}