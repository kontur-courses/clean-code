using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;
using Markdown.Transducer.ConverterTokenToHtml;

namespace Markdown.Transducer
{
    public class Transducer: ITransducer
    {
        private Dictionary<Type, IConverterTokenToHtml> convertersTokenToHtml = new Dictionary<Type, IConverterTokenToHtml>();
        public void Registred(IToken token, IConverterTokenToHtml converterTokenToHtml)
        {
            convertersTokenToHtml.Add(token.GetType(), converterTokenToHtml);
        }

        public string MakeHtmlString(string str, IToken[] tokens)
        {
            Array.Sort(tokens, (t1, t2) => t1.IndexTokenStart.CompareTo(t2.IndexTokenStart));
            var stringBuilder = new StringBuilder();
            var currentPosition = 0;
            foreach (var token in tokens)
            {
                stringBuilder.Append(str.Substring(currentPosition, token.IndexTokenStart - currentPosition));
                currentPosition += str.Substring(currentPosition, token.IndexTokenStart - currentPosition).Length;
                stringBuilder.Append(convertersTokenToHtml[token.GetType()].MakeConverter(token));
                currentPosition += token.Length;
            }

            stringBuilder.Append(str.Substring(currentPosition));
            return stringBuilder.ToString();
        }
    }
}