using System;
using System.Collections.Generic;
using Markdown.TokenConverters;

namespace Markdown
{
    public class Markdown
    {
        private IReadOnlyCollection<ITokenConverter> tokenConverters;
        private IReadOnlyCollection<ITokenGetter> tokenGetters;

        public Markdown()
        {
            tokenConverters = new List<ITokenConverter>
            {
                new HeaderTokenConverter(),
                new StrongTokenConverter(),
                new EmphasizedTokenConverter(),
                new TextTokenConverter()
            };
            tokenGetters = new ITokenGetter[]
            {
                new HeaderTokenGetter(),
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };
        }
        
        public string Render(string text)
        {
            var tokens = new TextParser(tokenGetters).GetTextTokens(text);//TODO заменить классы на статические 
            return new HTMLConverter(tokenConverters).GetHtml(tokens);
        }
    }
}