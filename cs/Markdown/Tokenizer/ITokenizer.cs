using System.Collections.Generic;

namespace Markdown.Tokenizer
{
    public interface ITokenizer
    {
        IEnumerable<IToken> MakeTokens(string text);
    }
}