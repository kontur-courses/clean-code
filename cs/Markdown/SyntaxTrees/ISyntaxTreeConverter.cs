using Markdown.Separators;

namespace Markdown.SyntaxTrees
{
    public interface ISyntaxTreeConverter
    {
        string Convert(SyntaxTree syntaxTree, ISeparatorConverter separatorConverter);
    }
}