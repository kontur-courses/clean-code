using Markdown.SeparatorConverters;

namespace Markdown.SyntaxAnalysis.SyntaxTreeConverters
{
    public interface ISyntaxTreeConverter
    {
        string Convert(SyntaxTree.SyntaxTree syntaxTree, ISeparatorConverter separatorConverter);
    }
}