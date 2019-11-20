using Markdown.SeparatorConverters;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeConverters
{
    public interface ISyntaxTreeConverter
    {
        string Convert(SyntaxTree syntaxTree, ISeparatorConverter separatorConverter);
    }
}