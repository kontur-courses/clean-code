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
            var terms = new StringToTermParser(input, new List<char>() {'_', '\\', '#'}).ParseByServiceSymbols();
            var resultTerms = new TermsAnalizator(input).AnalyseTerms(terms);
            var result = TermsToHtmlParser.ParseTermsToHtml(resultTerms, input, serviceSymbolHtml);
            return result;
        }
    }
}
