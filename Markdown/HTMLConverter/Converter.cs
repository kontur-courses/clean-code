using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.HTMLConverter
{
    public class Converter : IConverter
    {
        public static void GetContent(Token token, StringBuilder sb)
        {
            if (token.Children.Count == 0)
            {
                if (token.Property != TokenProperty.Link)
                {
                    sb.Append(Tags.Open[token.Property]);
                    sb.Append(token.Value);
                    sb.Append(Tags.Close[token.Property]);
                }
                else ConvertLink(token.Value, sb);
            }
            else
            {
                sb.Append(Tags.Open[token.Property]);
                foreach (var nextToken in token.Children)
                    GetContent(nextToken, sb);
                sb.Append(Tags.Close[token.Property]);
            }
            return;
        }

        public static void ConvertLink(string text, StringBuilder sb)
        {
            var i = 3;
            string name;
            string address;
            while (true)
            {
                if (text[i] == '+' && text[i - 1] == '+' && text[i - 2] == '+')
                {
                    name = text[..(i - 2)];
                    address = text[(i + 1)..];
                    break;
                }
                i++;
            }
            sb.Append(Tags.Open[TokenProperty.Link]);
            sb.Append(address);
            sb.Append('>');
            sb.Append(name);
            sb.Append(Tags.Close[TokenProperty.Link]);
        }

        public string Convert(DOM dom)
        {
            var result = new StringBuilder();
            foreach (var token in dom.Tokens)
                GetContent(token, result);
            return result.ToString();
        }
    }
}
