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
        public StringToTermParser(string input, List<char> serviceSymbols)
        {
            this.input = input;
            this.serviceSymbols = ImmutableList.Create(serviceSymbols.ToArray());
        }

        private readonly string input;
        private readonly ImmutableList<char> serviceSymbols;
        public IEnumerable<Term> ParseByServiceSymbols()
        {
            var result = new Stack<Term>();
            var splitString = input.Select((c, i) => Tuple.Create(c, i)).Where(t => serviceSymbols.Contains(t.Item1));
            var startIndex = 0;

            foreach (var serviceSymbol in splitString)
            {
                if (startIndex == serviceSymbol.Item2)
                    continue;
                AddTermsBetweenServiceSymbolsIndexes(result, startIndex, serviceSymbol.Item2);

                startIndex = serviceSymbol.Item2;
            }
            AddTermsBetweenServiceSymbolsIndexes(result, startIndex, input.Length);

            return result.Reverse();
        }

        private void AddTermsBetweenServiceSymbolsIndexes(Stack<Term> result, int startIndex, int serviceSymbolIndex)
        {
            if (serviceSymbols.Contains(input[startIndex]))
            {
                Term last;
                result.TryPop(out last);
                foreach (var term in TermsDeterminant.Determinate(last, new Term(startIndex, startIndex, input[startIndex].ToString()), input))
                    result.Push(term);
                if (startIndex + 1 <= serviceSymbolIndex - 1)
                    result.Push(new Term(startIndex + 1, serviceSymbolIndex - 1, "", false));
            }
            else
                result.Push(new Term(startIndex, serviceSymbolIndex - 1, "", false));
        }
    }
}
