using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Elements;

namespace Markdown.Parsers
{
    public class EmphasisParser
    {
        private static readonly IElementType[] PossibleElementTypes = new IElementType[]
        {
            UnderscoreElementType.Create(), DoubleUnderscoreElementType.Create()
        };

        private readonly string markdown;
        private readonly int startPosition;
        private readonly IElementType elementType;
        private int currentPosition;
        private List<MarkdownElement> innerElements;

        public EmphasisParser(string markdown, int startPosition, IElementType elementType)
        {
            this.markdown = markdown;
            this.startPosition = startPosition;
            this.elementType = elementType;
            currentPosition = startPosition;
            innerElements = new List<MarkdownElement>();
        }

        public MarkdownElement Parse()
        {
            while (currentPosition < markdown.Length)
            {
                if (elementType.IsClosingOfElement(markdown, currentPosition))
                    return new MarkdownElement(
                        elementType, markdown, startPosition, currentPosition, innerElements);

                var openingElementType = GetOpeningElementType(markdown, currentPosition);
                var closingElementType = GetClosingElementType(markdown, currentPosition);
                
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
                markdown,
                currentPosition + innerElementType.Indicator.Length,
                innerElementType);

            var innerElement = innerElementParser.Parse();

            if (innerElement.ElementType == BrokenElementType.Create())
                innerElements.AddRange(innerElement.InnerElements);
            else
                innerElements.Add(innerElement);
            currentPosition = innerElement.EndPosition + innerElement.ElementType.Indicator.Length;
        }

        private static IElementType GetOpeningElementType(string markdown, int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => type.IsOpeningOfElement(markdown, position));
        }

        private static IElementType GetClosingElementType(string markdown, int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => type.IsClosingOfElement(markdown, position));
        }
    }
}
