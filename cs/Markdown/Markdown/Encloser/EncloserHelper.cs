using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class EncloserHelper
    {
        private bool AreCellsVacant(int start, int end, bool[] vacant)
        {
            var isVacant = true;
            for (int i = start; i < end; ++i)
                isVacant = isVacant && vacant[i];
            return isVacant;
        }

        private void TakeVacantPlaces(int start, int end, bool[] vacant)
        {
            for (int i = start; i < end; ++i)
                vacant[i] = false;
        }

        public void ShiftTokens(Token[] array)
        {
            for (int i = 0; i < array.Length - 1; ++i)
                array[i] = array[i + 1];
        }

        private bool IsMdElement(Token[] curElement, string elementString, string sign) =>
            curElement.All(token => token.Type == TokenType.MdElement) && elementString == sign;

        private bool IsOpeningElement(Token[] curElement) =>
            curElement.All(token => token.MdPosition == MdPosition.Opening);

        private bool IsEnclosingElement(Token[] curElement, Stack<EncloserToken> enclosing) =>
            curElement.All(token => token.MdPosition == MdPosition.Enclosing) && enclosing.Count != 0;

        private void  EncloseOpenElement(int i, string sign, string elementString, bool[] vacant, EncloserToken[] encloserTokens)
        {
            var openElement = new EncloserToken(elementString, TokenType.MdElement,
                        MdPosition.Opening, i - sign.Length + 1, i + 1);
            openElement.IsClosed = true;
            encloserTokens[openElement.Start] = openElement;
            TakeVacantPlaces(openElement.Start, openElement.End, vacant);
        }

        private void PushElementForEnclosing(int i, string sign, string elementString, Stack<EncloserToken> enclosing) => 
            enclosing.Push(new EncloserToken(elementString, TokenType.MdElement,
                        MdPosition.Opening, i - sign.Length + 1, i + 1));

        private void EnclosePairOfEnclosedElements(int i, string sign, string elementString,
            Stack<EncloserToken> enclosing, EncloserToken[] encloserTokens, bool[] vacant)
        {
            var openingElement = enclosing.Pop();
            if (AreCellsVacant(openingElement.Start, openingElement.End, vacant))
            {
                var enclosingElement = new EncloserToken(elementString, TokenType.MdElement,
                    MdPosition.Enclosing, i - sign.Length + 1, i + 1);
                enclosingElement.IsClosed = true;
                openingElement.IsClosed = true;
                encloserTokens[openingElement.Start] = openingElement;
                encloserTokens[enclosingElement.Start] = enclosingElement;
                TakeVacantPlaces(openingElement.Start, openingElement.End, vacant);
                TakeVacantPlaces(enclosingElement.Start, enclosingElement.End, vacant);
            }
        }

        public void InspectForEnclosing(int i, string sign, bool[] vacant, Token[] curElement,
             Dictionary<string, MdElement> elementSigns, Stack<EncloserToken> enclosing, EncloserToken[] encloserTokens)
        {
            var elementString = string.Join("", curElement.Select(token => token.Value));
            if (IsMdElement(curElement, elementString, sign))
            {
                if (!elementSigns[sign].IsEnclosed)
                    EncloseOpenElement(i, sign, elementString, vacant, encloserTokens);
                else if (IsOpeningElement(curElement))
                    PushElementForEnclosing(i, sign, elementString, enclosing);
                else if (IsEnclosingElement(curElement, enclosing))
                    EnclosePairOfEnclosedElements(i, sign, elementString, enclosing, encloserTokens, vacant);
            }
        }
    }
}
