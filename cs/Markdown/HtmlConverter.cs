using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal static class HtmlConverter
    {
        internal static string ConvertToHtml(string inputString, IEnumerable<TagToken> tagsForChange)
        {
            var tagIndices = tagsForChange
                .ToDictionary(tag => tag.Index, tag => tag);
            
            var result = new StringBuilder();

            for (int i = 0; i < inputString.Length;)
            {
                if (tagIndices.ContainsKey(i))
                {
                    result.Append(tagIndices[i].TokenType == TagTokenType.Opening
                        ? $"<{tagIndices[i].MarkdownTagInfo.HtmlTagDesignation}>"
                        : $"</{tagIndices[i].MarkdownTagInfo.HtmlTagDesignation}>");
                    i += tagIndices[i].MarkdownTagInfo.MarkdownTagDesignation.Length;
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
