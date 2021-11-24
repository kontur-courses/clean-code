using Markdown.TokenizerLogic;
using System.Collections.Generic;

namespace Markdown
{
    public static class Tokenizer
    {
        public static IEnumerable<Token> ProcessMarkdown(string markdownText)
        {
            var rawTokens = TokenParser.ToRawTokens(markdownText);
            var composedTokens = TokenComposer.FilterRawTokens(rawTokens);
            var pairedTokens = TokenPairer.PairFilteredTokens(composedTokens);

            return pairedTokens;
        }
    }
}
