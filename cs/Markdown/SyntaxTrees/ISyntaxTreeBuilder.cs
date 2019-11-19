using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.SyntaxTrees
{
    public interface ISyntaxTreeBuilder
    {
        SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text);
    }
}