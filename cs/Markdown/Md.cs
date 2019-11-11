using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string sourceText)
        {
            FeaturesLoader.LoadFeatures();
            TokenInfo mainTokenInfo = ParseToTokens(sourceText);
            return ConvertToHtml(mainTokenInfo, sourceText);
        }

        private static TokenInfo ParseToTokens(string sourceText)
        {
            var context = new Context(sourceText);
            throw new NotImplementedException();
        }

        private static string ConvertToHtml(TokenInfo mainTokenInfo, string sourceText)
        {
            throw new NotImplementedException();
        }
    }
}