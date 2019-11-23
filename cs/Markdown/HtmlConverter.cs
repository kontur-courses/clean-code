using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal static class HtmlConverter
    {
        internal static string ConvertToHtml(string inputString, IEnumerable<TagToken> tagTokens)
        {
            var tagIndices = tagTokens
                .ToDictionary(tag => tag.Index, tag => tag);
            
            var result = new StringBuilder();

            for (int i = 0; i < inputString.Length;)
            {
                if (tagIndices.ContainsKey(i))
                {
                    result.Append(tagIndices[i].TokenType == TagTokenType.Opening
                        ? $"<{tagIndices[i].MarkdownTagInfo.HtmlTagDesignation}>"
                        : $"</{tagIndices[i].MarkdownTagInfo.HtmlTagDesignation}>");
                    i += tagIndices[i].MarkdownTagInfo.MarkdownTagOpenDesignation.Length;
                }
                else
                {
                    result.Append(inputString[i]);
                    i += 1;
                }
            }

            return result.ToString();
        }
    }
}
