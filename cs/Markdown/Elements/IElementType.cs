using Markdown.Analyzers;

namespace Markdown.Elements
{
    public interface IElementType
    {
        string Indicator { get; }
        bool CanContainElement(IElementType element);
        bool IsOpeningOfElement(SyntaxAnalysisResult syntaxAnalysisResult, int position);
        bool IsClosingOfElement(SyntaxAnalysisResult syntaxAnalysisResult, int position);
    }
}
