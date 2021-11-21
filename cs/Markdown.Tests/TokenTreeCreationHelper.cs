using System.Linq;
using Markdown.SyntaxParser;

namespace Markdown.Tests
{
    public class TokenTreeCreationHelper
    {
        public TokenTree[] GenerateFromText(params string[] words)
        {
            return words.Select(TokenTree.FromText).ToArray();
        }
    }
}