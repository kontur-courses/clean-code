using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Markdown
{
    public class Md : IMarkdown
    {
        private Converter converter;

        public Md(Converter converter)
        {
            this.converter = converter;
        }
        public Md(){}
        public string Render(string text)
        {
            if(text == null)
                throw new ArgumentException();
            
            converter = new Converter(text);
            var tokens = converter.GetTokens();
            var formattedTokens = converter.FormatToken(tokens);
            return converter.GetHTML(formattedTokens);
        }
    }
}