using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class EncloserHelper
    {
        public static void ShiftTokens(Token[] array)
        {
            for (int i = 0; i < array.Length - 1; ++i)
                array[i] = array[i + 1];
        }

        public static void InspectForEnclosing(int i, string sign, bool[] vacant, Token[] currentElement,
             Dictionary<string, MdElement> elementSigns, Stack<EncloserToken> enclosing, EncloserToken[] encloserTokens)
        {
            var elementString = string.Join("", currentElement.Select(token => token.Value));
            if (IsMdElement(currentElement, elementString, sign))
            {
                var start = i - sign.Length + 1;
                var end = i + 1;
                var encloserToken = new EncloserToken(elementString, TokenType.MdElement, start, end);
                if (!elementSigns[sign].IsEnclosed)
                    EncloseOpenElement(encloserToken, encloserTokens, vacant);
                else if (IsOpeningElement(currentElement))
                    PushTokenForEnclosing(encloserToken, enclosing);
                else if (IsEnclosingElement(currentElement, enclosing))
                    EnclosePairOfTokens(encloserToken, vacant, enclosing, encloserTokens);
            }
        }

        private static bool IsMdElement(Token[] currentElement, string elementString, string sign) =>
            currentElement.All(token => token.Type == TokenType.MdElement) && elementString == sign;

        private static bool IsOpeningElement(Token[] currentElement) =>
            currentElement.All(token => token.MdPosition == MdPosition.Opening);

        private static bool IsEnclosingElement(Token[] currentElement, Stack<EncloserToken> enclosing) =>
            currentElement.All(token => token.MdPosition == MdPosition.Enclosing) && enclosing.Count != 0;

        private static bool AreVacantPlaces(EncloserToken token, bool[] vacant)
        {
            var start = token.Start;
            var end = token.End;
            var isVacant = true;
            for (int i = start; i < end; ++i)
                isVacant = isVacant && vacant[i];
            return isVacant;
        }

        private static void TakeVacantPlaces(EncloserToken token, bool[] vacant)
        {
            var start = token.Start;
            var end = token.End;
            for (int i = start; i < end; ++i)
                vacant[i] = false;
        }

        private static void  EncloseOpenElement(EncloserToken openToken, EncloserToken[] encloserTokens, bool[] vacant)
        {
            if (AreVacantPlaces(openToken, vacant))
            {
                openToken.MdPosition = MdPosition.Opening;
                openToken.IsClosed = true;
                encloserTokens[openToken.Start] = openToken;
                TakeVacantPlaces(openToken, vacant);
            }
        }

        private static void PushTokenForEnclosing(EncloserToken openingToken, Stack<EncloserToken> enclosing)
        {
            openingToken.MdPosition = MdPosition.Opening;
            enclosing.Push(openingToken);
        }

        private static void EnclosePairOfTokens(EncloserToken enclosingToken, bool[] vacant,
            Stack<EncloserToken> enclosing, EncloserToken[] encloserTokens)
        {
            var openingElement = enclosing.Pop();
            if (AreVacantPlaces(enclosingToken, vacant))
            {
                enclosingToken.MdPosition = MdPosition.Enclosing;
                enclosingToken.IsClosed = true;
                openingElement.IsClosed = true;
                encloserTokens[openingElement.Start] = openingElement;
                encloserTokens[enclosingToken.Start] = enclosingToken;
                TakeVacantPlaces(openingElement, vacant);
                TakeVacantPlaces(enclosingToken, vacant);
            }
        }
    }
}
