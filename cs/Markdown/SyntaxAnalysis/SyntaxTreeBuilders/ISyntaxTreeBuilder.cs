using System.Collections.Generic;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;
using Markdown.Tokenization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders
{
    public interface ISyntaxTreeBuilder
    {
        SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text);
    }
}