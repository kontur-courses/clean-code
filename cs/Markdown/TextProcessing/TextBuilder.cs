using System.Collections.Generic;
using System.Text;
using Markdown.TokenEssences;

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

        public string BuildText(List<string> paragraphs)
        {
            var strBuilder = new StringBuilder();
            for(int i = 0; i<paragraphs.Count;i++)
            {
                strBuilder.Append(paragraphs[i]);
                if(i!= paragraphs.Count-1)
                strBuilder.Append("\r\n\r\n");
            }
            return strBuilder.ToString();
        }

        public string BuildTokenValue(IToken token)
        {
            if (token.Value.Length == 0) return "";
            if (token.TypeToken == TypeToken.Simple)
                return token.Value;
            return string.Format(@"<{0}>{1}</{0}>", token.TypeToken.ToString().ToLower(), token.Value);
        }
    }
}