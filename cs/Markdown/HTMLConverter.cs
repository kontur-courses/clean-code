using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.TokenConverters;

namespace Markdown
{
    public class HTMLConverter
    {
        private IReadOnlyCollection<ITokenConverter> tokenConverters;

        public HTMLConverter(IReadOnlyCollection<ITokenConverter> tokenConverters)
        {
            this.tokenConverters = tokenConverters;
        }
        public string GetHtmlString(IReadOnlyCollection<TextToken> textTokens)
        {
            var stringedHtml = new StringBuilder();
            foreach (var token in textTokens)
            {
                foreach (var tokenConverter in tokenConverters)
                {
                    var currentText = tokenConverter.ConvertTokenToString(token, tokenConverters);
                    if(currentText == null) continue;
                    stringedHtml.Append(currentText);
                }
            }
            return stringedHtml.ToString();
        }
    }
}