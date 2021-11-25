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
                    return GetTermsWhenInteractingWithTitleTerm(lastTerm, currentTerm, input.Length);
                case "\\":
                    return GetTermsWhenInteractingWithShieldingTerm(lastTerm, currentTerm);
                case "_":
                    return GetTermsWhenInteractingWithUnderscoreTerm(lastTerm, currentTerm, input);
                default:
                    throw new ArgumentException();
            }
        }

        private static void ConvertTermInRegular(Term currentTerm)
        {
            currentTerm.Close();
            currentTerm.ChangeServiseSymbol("");
        }

        private static char GetCharOrDefault(string input, int index, char defaultV)
        {
            if (index >= input.Length || index < 0)
                return defaultV;
            else
                return input[index];
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithTitleTerm(Term lastTerm, Term currentTerm,int rightBorder)
        {
            var result = new List<Term>();

            if (lastTerm == null)
            {
                result.Add(new Term(0, rightBorder, currentTerm.ServiceSymbol, false));
                return result;
            }
            if (lastTerm.StartIndex == 0 && lastTerm.ServiceSymbol == "\\")
            {
                result.Add(new Term(currentTerm.StartIndex, currentTerm.EndIndex, "", false));
                return result;
            }
             
            ConvertTermInRegular(currentTerm);
            result.Add(lastTerm);
            result.Add(currentTerm);
            return result;
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithShieldingTerm(Term lastTerm, Term currentTerm)
        {
            var result = new List<Term>();

            if (lastTerm == null)
            {
                result.Add(currentTerm);
                return result;
            }

            if (lastTerm.ServiceSymbol.Equals("\\"))
            {
                ConvertTermInRegular(currentTerm);
                result.Add(currentTerm);
                return result;
            }
            
            result.Add(lastTerm);
            result.Add(currentTerm);
            return result;
        }

        private static IEnumerable<Term> GetTermsWhenInteractingWithUnderscoreTerm(Term lastTerm, Term currentTerm, string input)
        {
            var result = new List<Term>();

            if (lastTerm == null)
            {
                var nextSimb = GetCharOrDefault(input, currentTerm.StartIndex + 1, ' ');
                if (char.IsWhiteSpace(nextSimb))
                {
                    ConvertTermInRegular(currentTerm);
                }
                result.Add(currentTerm);
                return result;
            }
           
            switch (lastTerm.ServiceSymbol)
            {
                case "_":
                    result.Add(new Term(lastTerm.StartIndex, currentTerm.EndIndex, "__"));
                    return result;
                case "\\":
                    result.Add(new Term(currentTerm.StartIndex, currentTerm.EndIndex, "", false));
                    return result;
                default:
                    result.Add(lastTerm);
                    var previousSimb = GetCharOrDefault(input, currentTerm.StartIndex - 1, ' ');
                    var nextSimb = GetCharOrDefault(input, currentTerm.StartIndex + 1, ' ');
                    if (char.IsWhiteSpace(previousSimb) && char.IsWhiteSpace(nextSimb))
                        ConvertTermInRegular(currentTerm);
                    result.Add(currentTerm);
                    return result;
            }
        }
    }
}
