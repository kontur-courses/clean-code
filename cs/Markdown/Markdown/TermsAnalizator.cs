using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class TermsAnalizator
    {
        private static IEnumerable<Term> CombineTerms(Term open, Term closed, string input)
        {
            switch (open.ServiceSymbol)
            {
                case "\\":
                    yield return new Term(open.StartIndex, closed.StartIndex, "", false);
                    if (closed.ServiceSymbol.Length > 1)
                        yield return new Term(closed.EndIndex, closed.EndIndex, closed.ServiceSymbol[1].ToString());
                    break;
                case "#":
                    break;
                case "_":
                    if (closed.ServiceSymbol.Equals("_"))
                    {
                        if (open.InnerTerms.Any())
                        {
                            open.EndIndex = closed.StartIndex;
                            open.Close();
                        }
                        else
                        {
                            open.EndIndex = closed.StartIndex;
                            open.ChangeServiseSymbol("__");
                        }
                        yield return open;
                    }
                    break;
                case "__":
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static IEnumerable<Term> AnalyseTerms(IEnumerable<Term> terms, string input)
        {
            var stack = new Stack<Term>();
            var result = new List<Term>();

            foreach (var term in terms)
            {
                Term lastTerm;
                if (stack.TryPeek(out lastTerm))
                {
                    if (term.IsOpen)
                    {
                        stack.Pop();
                        foreach (var t in CombineTerms(lastTerm, term, input))
                        {
                            if (t.IsOpen)
                                stack.Push(t);
                            else
                                result.Add(t);
                        }
                    }
                    else
                    {
                        lastTerm.InnerTerms.Push(term);
                    }

                }
                else
                {
                    if (term.IsOpen)
                        stack.Push(term);
                    else
                    {
                        result.Add(term);
                    }
                }
            }

            foreach (var t in stack)
                result.Add(t);

            return result;
        }
    }
}
