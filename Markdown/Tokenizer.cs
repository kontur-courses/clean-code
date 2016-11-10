using System.Collections.Generic;

namespace Markdown
{
    public class Tokenizer
    {
        public IEnumerable<Token> SplitToTokens(string text, IEnumerable<IShell> shells)
        {
            return new List<Token> {new Token(text)};
        }
    }
}
