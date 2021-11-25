using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class TermsToHtmlParser
    {
        public static string ParseTermsToHtml(IEnumerable<Term> terms, string input, Dictionary<string, string> serviceSymbolTag)
        {
            var result = new StringBuilder();
            var closedTag = new HashSet<Term>();

            for(var i = 0; i <= input.Length; i++)
            {
                var borderTerms = terms.Where(term => term.StartIndex == i || term.EndIndex == i);
                foreach(var term in borderTerms)
                {
                    if (serviceSymbolTag.ContainsKey(term.ServiceSymbol))
                    {
                        if(!closedTag.Contains(term))
                            result.Append($"<{serviceSymbolTag[term.ServiceSymbol]}>");
                        else
                            result.Append($"</{serviceSymbolTag[term.ServiceSymbol]}>");
                    }
                    else if(!closedTag.Contains(term))
                        result.Append(input.Substring(term.StartIndex, term.EndIndex - term.StartIndex + 1));

                    closedTag.Add(term);
                }
            }
            return result.ToString();
        }
    }
}
