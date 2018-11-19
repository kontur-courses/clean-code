using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Markups;

namespace Markdown
{
    class MdParser
    {
        public string GetHtml(string text, List<Markup> markups)
        {
            var result = new StringBuilder();
            var tokenReader = new TokenReader(text, markups);
            while (tokenReader.HasMoreTokens())
            {
                var token = tokenReader.NextToken();
                if (token.HasMarkup())
                {
                    var nestedMarkups = markups.Where(m => token.Markup.Contains(m.GetType())).ToList();
                    var convertedTokenText = GetHtml(token.Text, nestedMarkups);
                    result.Append(token.ConvertToHtml(convertedTokenText));
                }
                else
                {
                    result.Append(token.Text);
                }
            }
            return result.ToString().RemoveEscapes(markups);
        }
    }
}
