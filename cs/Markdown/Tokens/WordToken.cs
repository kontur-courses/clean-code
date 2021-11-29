using System.Collections.Generic;
using System.Linq;
using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class WordToken: IToken
    {
        public string Word { get; init; }

        public bool ContainsDigits;

        public WordToken(List<CharToken> chars)
        {
            Word = string.Join("", chars.Select(x => x.Symbol));
            ContainsDigits = chars.Any(x => x.IsDigit);
        }

        public WordToken(string word)
        {
            Word = word;
            ContainsDigits = word.Any(char.IsDigit);
        }

        public INode ToNode()
        {
            return new StringNode(Word);
        }
    }
}