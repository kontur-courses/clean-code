using System;
using Markdown.Analyzers;

namespace Markdown.Elements
{
    public abstract class EmphasisTypeBase : IElementType
    {
        public abstract string Indicator { get; }

        public virtual bool IsOpeningOfElement(SyntaxAnalysisResult syntaxAnalysisResult, int position)
        {
            int positionAfterIndicator = position + Indicator.Length;
            return IsIndicatorAt(syntaxAnalysisResult, position) &&
                   syntaxAnalysisResult.Markdown.IsNonWhitespaceAt(positionAfterIndicator);
        }

        public virtual bool IsClosingOfElement(SyntaxAnalysisResult syntaxAnalysisResult, int position)
        {
            return IsIndicatorAt(syntaxAnalysisResult, position) &&
                   syntaxAnalysisResult.Markdown.IsNonWhitespaceAt(position - 1);
        }

        public abstract bool CanContainElement(IElementType element);
        public abstract bool IsIndicatorAt(SyntaxAnalysisResult syntaxAnalysisResult, int position);
    }
}
