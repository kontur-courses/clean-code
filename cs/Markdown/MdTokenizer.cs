using System.Collections.Generic;
using Markdown.Tests;

namespace Markdown
{
    public class MdTokenizer
    {
        private readonly MdTokenParser parser = new MdTokenParser();
        private readonly MdTokenFixer fixer = new MdTokenFixer();

        public List<Token> Tokenize(string paragraph)
        {
            var tokens = parser.Tokenize(paragraph);
            return fixer.FixTokens(tokens);
        }
    }
}