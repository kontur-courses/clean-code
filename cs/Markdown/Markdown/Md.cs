using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string input)
        {
            var serviceSymbolHtml = new Dictionary<string, string>
            {
                {"#","h1"},
                {"_","em"},
                {"__","strong"}
            };
            var terms = StringToTermParser.ParseByServiceSymbols(input, new List<char>() {'_', '\\', '#'});
            var resultTerms = TermsAnalizator.AnalyseTerms(terms, input);
            var result = TermsToHtmlParser.ParseTermsToHtml(resultTerms, input, serviceSymbolHtml);
            return result;
        }
    }
}
