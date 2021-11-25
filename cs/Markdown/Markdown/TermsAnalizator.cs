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
            Term lastTerm;
            if (stack.TryPeek(out lastTerm))
            {
                if (ArePair(lastTerm, term))
                {
                    lastTerm.EndIndex = term.EndIndex;
                    lastTerm.Close();
                    stack.Pop();
                    result.Add(lastTerm);
                    if (lastTerm.ServiceSymbol == "_")
                    {
                        ExcludeInternalTerm(result, excludeTerms, lastTerm, "__");
                    }
                }
                else
                    stack.Push(term);
            }
            else
                stack.Push(term);
        }

        private void ExcludeInternalTerm(List<Term> result, List<Term> excludeTerms, Term lastTerm, string excludeServiceSymbol)
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
                    excludeTerms.Add(term);
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
                var innerText = input.Substring(openingTerm.EndIndex + openingTerm.ServiceSymbol.Length,
                    closingTerm.StartIndex - openingTerm.EndIndex - openingTerm.ServiceSymbol.Length);

                if(IsInsideText(openingTerm) || IsInsideText(closingTerm))
                    return !innerText.Contains(" ") && !innerText.Where(c => char.IsDigit(c)).Any();

                return 
                    input[openingTerm.StartIndex + openingTerm.ServiceSymbol.Length] != ' ' 
                    && input[closingTerm.StartIndex - 1] != ' ';
            }
            return false;
        }
    }
}
