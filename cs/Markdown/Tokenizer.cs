using System.Collections.Generic;
using Markdown.TokenizerLogic;

namespace Markdown
{
    public static class Tokenizer
    {
        public static IEnumerable<Token> ProcessMarkdown(string markdownText)
        {
            var rawTokens = TokenParser.ToRawTokens(markdownText);
            var filteredTokens = TokenFilter.FilterRawTokens(rawTokens);
            var pairedTokens = TokenPairer.PairFilteredTokens(filteredTokens);

            return pairedTokens;
        }
    }   
}
