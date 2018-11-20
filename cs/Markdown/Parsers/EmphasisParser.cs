using System.Collections.Generic;
using System.Linq;
using Markdown.Analyzers;
using Markdown.Elements;

namespace Markdown.Parsers
{
    public class EmphasisParser
    {
        private static readonly IElementType[] PossibleElementTypes = new IElementType[]
        {
            SingleUnderscoreElementType.Create(), DoubleUnderscoreElementType.Create()
        };

        private readonly string markdown;
        private readonly int startPosition;
        private readonly IElementType elementType;
        private int currentPosition;
        private readonly List<MarkdownElement> innerElements;
        private readonly SyntaxAnalysisResult syntaxAnalysisResult;

        public EmphasisParser(SyntaxAnalysisResult syntaxAnalysisResult, int startPosition, 
            IElementType elementType)
        {
            markdown = syntaxAnalysisResult.Markdown;
            this.startPosition = startPosition;
            this.elementType = elementType;
            currentPosition = startPosition;
            innerElements = new List<MarkdownElement>();
            this.syntaxAnalysisResult = syntaxAnalysisResult;
        }

        public MarkdownElement Parse()
        {
            while (currentPosition < markdown.Length)
            {
                if (elementType.IsClosingOfElement(syntaxAnalysisResult, currentPosition))
                    return new MarkdownElement(
                        elementType, markdown, startPosition, currentPosition, innerElements);

                var openingElementType = GetOpeningElementType(currentPosition);
                var closingElementType = GetClosingElementType(currentPosition);
                
                if (openingElementType != null && elementType.CanContainElement(openingElementType))
                    HandleInnerElement(openingElementType);

                else if (openingElementType != null || 
                         closingElementType != null && elementType != RootElementType.Create())
                    return new MarkdownElement(
                        BrokenElementType.Create(), markdown, startPosition, currentPosition, innerElements);
                else
                    currentPosition++;
            }

            var isBrokenElement = elementType != RootElementType.Create();
            
            return new MarkdownElement(
                isBrokenElement ? BrokenElementType.Create() : elementType, 
                markdown, startPosition, currentPosition, innerElements);
        }

        private void HandleInnerElement(IElementType innerElementType)
        {
            var innerElementParser = new EmphasisParser(
                syntaxAnalysisResult,
                currentPosition + innerElementType.Indicator.Length,
                innerElementType);

            var innerElement = innerElementParser.Parse();

            if (innerElement.ElementType == BrokenElementType.Create())
                innerElements.AddRange(innerElement.InnerElements);
            else
                innerElements.Add(innerElement);
            currentPosition = innerElement.EndPosition + innerElement.ElementType.Indicator.Length;
        }

        private IElementType GetOpeningElementType(int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => type.IsOpeningOfElement(syntaxAnalysisResult, position));
        }

        private IElementType GetClosingElementType(int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => type.IsClosingOfElement(syntaxAnalysisResult, position));
        }
    }
}
