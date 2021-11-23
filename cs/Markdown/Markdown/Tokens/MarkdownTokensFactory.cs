using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public static class MarkdownTokensFactory
    {
        public static IEnumerable<IToken> GetTokens()
        {
            yield return new ItalicToken();
            yield return new BoldToken();
            yield return new HeaderToken();
        }
    }
}