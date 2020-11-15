using System;
using System.Collections.Generic;
using Markdown.TokenConverters;

namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";
            var tokenGetters = new List<ITokenGetter>()
            {
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };
            
            var tokenConverters = new List<ITokenConverter>
            {
                new StrongTokenConverter(),
                new EmphasizedTokenConverter(),
                new TextTokenConverter()
            };
            
            var textParser = new TextParser(tokenGetters);

            var textTokens = textParser.GetTextTokens(text);

            var htmlConverter = new HTMLConverter(tokenConverters);
            var htmlString = htmlConverter.GetHTMLString(textTokens);
            Console.WriteLine(htmlString);
        }
    }
}