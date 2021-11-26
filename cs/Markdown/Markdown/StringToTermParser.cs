using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StringToTermParser
    {
        public StringToTermParser(List<char> serviceSymbols)
        {
            this.serviceSymbols = ImmutableList.Create(serviceSymbols.ToArray());
        }

        private readonly ImmutableList<char> serviceSymbols;

        public IEnumerable<Term> ParseByServiceSymbols(string input)
        {
            var result = new Stack<Term>();
            var start = 0;

            for (var i = 0; i < input.Length; i++)
                if (serviceSymbols.Contains(input[i]))
                {
                    if (start != i)
                        result.Push(new Term(start, i - 1, "", false));

                    var nextSymb = TryGetSymbol(input, i + 1);

                    Term currentTerm;
                    if (input[i] == '_' && nextSymb == '_')
                    {
                        currentTerm = new Term(i, i + 1, "__");
                        i++;
                    }
                    else if(input[i] == '\\')
                        currentTerm = new Term(i, i, input[i].ToString(), false);
                    else
                        currentTerm = new Term(i, i, input[i].ToString());

                    result.TryPop(out var lastTerm);
                    foreach (var term in TermsDeterminant.Determinate(lastTerm, currentTerm, input))
                        result.Push(term);

                    start = i + 1;
                }

            if (start < input.Length)
                result.Push(new Term(start, input.Length - 1, "", false));

            return result.Reverse();
        }

        private static char? TryGetSymbol(string input, int index)
        {
            if (index >= 0 && index < input.Length)
                return input[index];
            return null;
        }
        
    }
}
