using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringToTermParser
    {

        //Подумал что будет лучше разбивать на блоки по принципу - _a_ - будет 3 отдельных блока(_, a, _)
        public static IEnumerable<Term> ParseByServiceSymbols(string input, List<char> serviceSymbols)
        {
            var splitString = input.Select((c, i) => Tuple.Create(c, i)).Where(t => serviceSymbols.Contains(t.Item1));
            var startIndex = 0;

            foreach (var c in splitString)
            {
                if (startIndex == c.Item2)
                    continue;
                if (serviceSymbols.Contains(input[startIndex]))
                {
                    yield return new Term(startIndex, startIndex, input[startIndex].ToString());
                    if (startIndex + 1 <= c.Item2 - 1)
                        yield return new Term(startIndex + 1, c.Item2 - 1, "", false);
                }
                else
                    yield return new Term(startIndex, c.Item2 - 1, "", false);

                startIndex = c.Item2;
            }

            if (serviceSymbols.Contains(input[startIndex]))
            {
                yield return new Term(startIndex, startIndex, input[startIndex].ToString());
                if (startIndex != input.Length - 1)
                    yield return new Term(startIndex + 1, input.Length - 1, "", false);
            }
            else
                yield return new Term(startIndex, input.Length - 1, "", false);
        }
    }
}
