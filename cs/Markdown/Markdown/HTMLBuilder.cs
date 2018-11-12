using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class HtmlBuilder
    {
        public static string Build(IEnumerable<Token> tokens)
        {
            var htmlText = new StringBuilder();
            foreach (var token in tokens)
            {
                htmlText.Append(token.Mark.OpeningTag);
                htmlText.Append(token.Text);
                htmlText.Append(token.Mark.ClosingTag);
            }

            return htmlText.ToString();
        }

        public static string RemoveRedundantBackSlashes(string text, IEnumerable<Mark> marks)
        {
            var backSlashesCount = 0;
            var builder = new StringBuilder(text);
            var curIndex = 0;
            foreach (var oneSymbolMark in marks.Where(m => m.Sign.Length == 1))
            {
                while (curIndex < builder.Length)
                {
                    if (builder[curIndex] == '\\')
                        backSlashesCount++;
                    else
                    {
                        if (builder[curIndex].ToString() == oneSymbolMark.Sign && backSlashesCount % 2 == 1)
                        {
                            builder.Remove(curIndex - 1, 1);
                            curIndex--;
                        }
                        backSlashesCount = 0;
                    }
                    curIndex++;
                }
            }
            builder.Replace(@"\\", @"\");
            return builder.ToString();
        }
    }
}
