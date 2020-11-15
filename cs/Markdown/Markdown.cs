using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "_aa_bc";

            var tokenGetters = new List<ITokenGetter>()
            {
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };

            var textParser = new TextParser(tokenGetters);

            var textTokens = textParser.GetTextTokens(text);

            var htmlConverter = new HTMLConverter();
           // var htmlString = htmlConverter.GetHTMLString(textTokens);
            //var a = htmlString;
        }
    }
}