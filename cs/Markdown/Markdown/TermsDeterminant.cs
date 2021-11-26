using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class TermsDeterminant
    {
        public static IEnumerable<Term> Determinate(Term lastTerm, Term currentTerm, string input) 
        {
            switch (currentTerm.ServiceSymbol)
            {
                case "#":
                    return GetTermsWhenInteractingWithTitleTerm(lastTerm, currentTerm, input);
                case "\\":
                    return GetTermsWhenInteractingWithShieldingTerm(lastTerm, currentTerm);
                case "_":
                    return GetTermsWhenInteractingWithUnderscoreTerm(lastTerm, currentTerm, input);
                case "__":
                    return GetTermsWhenInteractingWithUnderscoreTerm(lastTerm, currentTerm, input);
                default:
                    var result = new List<Term>();
                    if (lastTerm != null)
                        result.Add(lastTerm);
                    result.Add(currentTerm);
                    return result;
            }
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithTitleTerm(Term lastTerm, Term currentTerm, string input)
        {
            if (lastTerm == null)
            {
                if (!string.IsNullOrWhiteSpace(input.Substring(1, input.Length - 1)))
                {
                    yield return new Term(0, input.Length, currentTerm.ServiceSymbol, false);
                    yield break;
                }
                yield return currentTerm;
                yield break;
            }

            if (lastTerm.ServiceSymbol == "\\")
            {
                yield return currentTerm;
                yield break;
            }

            yield return lastTerm;
            yield return currentTerm;
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithShieldingTerm(Term lastTerm, Term currentTerm)
        {
            if (lastTerm == null || lastTerm.ServiceSymbol.Equals("\\"))
            {
                yield return currentTerm;
                yield break;
            }

            yield return lastTerm;
            yield return currentTerm;
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithUnderscoreTerm(Term lastTerm, Term currentTerm, string input)
        {
            if (lastTerm == null)
            {
                yield return currentTerm;
                yield break;
            }

            if (lastTerm.ServiceSymbol.Equals("\\"))
            {
                yield return new Term(currentTerm.StartIndex, currentTerm.EndIndex, "", false);
                yield break;
            }
            
            yield return lastTerm;
            yield return currentTerm;
        }
    }
}
