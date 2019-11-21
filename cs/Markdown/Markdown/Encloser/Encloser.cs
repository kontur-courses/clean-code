using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Encloser
    {
        private Dictionary<string, MdElement> elementSigns;

        public Encloser(Dictionary<string, MdElement> elementSigns)
        {
            this.elementSigns = elementSigns;
        }

        public List<Token> Enclose(List<Token> initialTokens)
        {
            var vacant = new bool[initialTokens.Count];
            var encloserTokens = new EncloserToken[initialTokens.Count];
            SetVacantPlaces(vacant, initialTokens, encloserTokens);
            EncloseTokens(initialTokens, vacant, encloserTokens);
            return GetResult(encloserTokens, initialTokens);
        }

        private void SetVacantPlaces(bool[] vacant, List<Token> initialTokens, EncloserToken[] encloserTokens)
        {
            for (int i = 0; i < initialTokens.Count; ++i)
            {
                if (initialTokens[i].Type == TokenType.MdElement)
                    vacant[i] = true;
                else
                    encloserTokens[i] = initialTokens[i] as EncloserToken;
            }
        }

        private void EncloseTokens(List<Token> initialTokens, bool[] vacant, EncloserToken[] encloserTokens)
        {
            var suitableSigns = elementSigns.Keys
                .OrderByDescending(sign => sign.Length)
                .Where(sign => sign.Length <= initialTokens.Count);
            foreach (var sign in suitableSigns)
                EncloseTokensWithSameSign(sign, vacant, initialTokens, encloserTokens);
        }

        private void EncloseTokensWithSameSign(string sign, bool[] vacant,
            List<Token> initialTokens, EncloserToken[] encloserTokens)
        {
            var currentElement = initialTokens.Take(sign.Length).ToArray();
            var enclosing = new Stack<EncloserToken>();
            for (int i = sign.Length - 1; i < initialTokens.Count - 1; ++i)
            {
                EncloserHelper.InspectForEnclosing(i, sign, vacant, currentElement,
                    elementSigns, enclosing, encloserTokens);
                EncloserHelper.ShiftTokens(currentElement);
                currentElement[currentElement.Length - 1] = initialTokens[i + 1];
            }
            EncloserHelper.InspectForEnclosing(initialTokens.Count - 1, sign, vacant, currentElement,
                elementSigns, enclosing, encloserTokens);
        }

        private List<Token> GetResult(EncloserToken[] encloserTokens, List<Token> initialTokens)
        {
            var result = new List<Token>();
            var skip = 0;
            for (int i = 0; i < encloserTokens.Length; ++i)
            {
                if (skip != 0)
                {
                    skip--;
                    continue;
                }
                var enclosedToken = encloserTokens[i];
                if (enclosedToken == null || initialTokens[i].Type != TokenType.MdElement)
                    result.Add(initialTokens[i]);
                else
                {
                    enclosedToken.MdType = elementSigns[enclosedToken.Value];
                    skip = enclosedToken.End - enclosedToken.Start - 1;
                    result.Add(enclosedToken);
                }
            }
            return result;
        }
    }
}
