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
            foreach (var term in terms)
            {
                if (!term.IsOpen && term.ServiceSymbol != "")
                {
                    result.Append(ExpandChildToken(term, input, serviceSymbolTag));
                }
                else
                {
                    result.Append(term.GetInnerText(input));
                }
            }

            return result.ToString();
        }

        private static string ExpandChildToken(Term parent, string input, Dictionary<string,string> serviceSymbolTag)
        {
            var result = new StringBuilder();

            var childText = new StringBuilder();

            foreach (var e in parent.InnerTerms)
                childText.Append(ExpandChildToken(e, input, serviceSymbolTag));

            if (!parent.IsOpen && parent.ServiceSymbol != "")
            {
                result.Append($"<{serviceSymbolTag[parent.ServiceSymbol]}>{childText}</{serviceSymbolTag[parent.ServiceSymbol]}>");
            }
            else
            {
                result.Append(parent.GetInnerText(input));
            }



            return result.ToString();
        }
    }
}
