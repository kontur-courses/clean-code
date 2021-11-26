using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TermsAnalizator
    {
        public TermsAnalizator(string input)
        {
            this.input = input;
        }

        private readonly string input;

        public IEnumerable<Term> AnalyseTerms(IEnumerable<Term> terms)
        {
            var stack = new Stack<Term>();
            var result = new List<Term>();

            var excludeTerms = new List<Term>();

            foreach (var term in terms)
            {
                if (term.IsOpen)
                {
                    AnalyseServiceTerm(stack, result, excludeTerms, term);
                }
                else
                    result.Add(term);
            }

            foreach (var term in stack)
            {
                term.Close();
                term.ChangeServiseSymbol("");
                result.Add(term);
            }

            return result.Except(excludeTerms);
        }

        private void AnalyseServiceTerm(Stack<Term> stack, List<Term> result, List<Term> excludeTerms, Term term)
        {
            if (stack.TryPeek(out var lastTerm))
            {
                if (ArePair(lastTerm, term))
                {
                    lastTerm.EndIndex = term.EndIndex;
                    lastTerm.Close();
                    stack.Pop();
                    result.Add(lastTerm);
                    if (lastTerm.ServiceSymbol == "_")
                    {
                        excludeTerms.AddRange(ExcludeInternalTerm(result, lastTerm, "__"));
                    }
                }
                else
                    stack.Push(term);
            }
            else
                stack.Push(term);
        }

        private IEnumerable<Term> ExcludeInternalTerm(List<Term> result, Term lastTerm, string excludeServiceSymbol)
        {
            var internalTerms = result
                .Where
                (
                    term => term.StartIndex > lastTerm.StartIndex 
                    && term.EndIndex < lastTerm.EndIndex 
                    && term.ServiceSymbol == excludeServiceSymbol
                );

            foreach (var excludeTerm in internalTerms)
            {
                excludeTerm.ChangeServiseSymbol("");
                foreach (var term in result.Where(term => term.StartIndex > excludeTerm.StartIndex && term.EndIndex < excludeTerm.EndIndex))
                    yield return term;
            }
        }

        private bool IsInsideText(Term term)
        {
            var leftBorder = term.StartIndex - 1;
            var rightBorder = term.StartIndex + term.ServiceSymbol.Length;

            return leftBorder >= 0
                && rightBorder < input.Length
                && char.IsLetterOrDigit(input[leftBorder])
                && char.IsLetterOrDigit(input[rightBorder]);
        }

        private bool ArePair(Term openingTerm, Term closingTerm)
        {
            if (!openingTerm.ServiceSymbol.Equals(closingTerm.ServiceSymbol))
                return false;
            if (openingTerm.ServiceSymbol != "")
            {
                var innerText = input.Substring(openingTerm.StartIndex + openingTerm.ServiceSymbol.Length,
                    closingTerm.StartIndex - openingTerm.StartIndex - openingTerm.ServiceSymbol.Length);

                if (IsInsideText(openingTerm) || IsInsideText(closingTerm))
                    return !innerText.Contains(" ") && !innerText.Any(char.IsDigit);

                return 
                    input[openingTerm.StartIndex + openingTerm.ServiceSymbol.Length] != ' ' 
                    && input[closingTerm.StartIndex - 1] != ' ' 
                    && !(string.IsNullOrEmpty(innerText) || string.IsNullOrWhiteSpace(innerText));
            }
            return false;
        }
    }
}
