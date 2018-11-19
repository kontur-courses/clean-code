using System.Collections.Generic;
using System.Text;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TextBuilder
    {
        public string BuildText(List<IToken> tokens)
        {
            var strBuilder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.TypeToken == TypeToken.Simple)
                    strBuilder.Append(token.Value);
                else
                    strBuilder.AppendFormat("<{0}>{1}</{0}>", token.TypeToken.ToString().ToLower(), token.Value);
            }
            return strBuilder.ToString();
        }

        public string BuildToken(IToken token)
        {
            if (token.Value.Length == 0) return "";
            if (token.TypeToken == TypeToken.Simple)
                return token.Value;
            return string.Format(@"<{0}>{1}</{0}>", token.TypeToken.ToString().ToLower(), token.Value);
        }
    }
}