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
        public string GetHTMLString(IReadOnlyCollection<TextToken> textTokens, IReadOnlyCollection<ITokenConverter> tokenConverters)
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