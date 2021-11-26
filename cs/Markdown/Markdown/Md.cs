using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class Md
    {
        private static Dictionary<string, string> serviceSymbolHtml = new Dictionary<string, string>
            {
                {"#","h1"},
                {"_","em"},
                {"__","strong"}
            };

        private static List<char> serviceSymbols = new List<char>() { '_', '\\', '#' };
        public static string Render(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            var terms = new StringToTermParser(serviceSymbols).ParseByServiceSymbols(input);
            var resultTerms = new TermsAnalizator(input).AnalyseTerms(terms);
            var result = TermsToHtmlParser.ParseTermsToHtml(resultTerms, input, serviceSymbolHtml);
            return result;
        }
    }
}
