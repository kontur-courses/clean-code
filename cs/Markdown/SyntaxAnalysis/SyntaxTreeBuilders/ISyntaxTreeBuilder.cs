using System.Collections.Generic;
using Markdown.Tokenization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders
{
    public interface ISyntaxTreeBuilder
    {
        SyntaxTree.SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text);
    }
}