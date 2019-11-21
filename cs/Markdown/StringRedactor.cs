using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal static class StringRedactor
    {
        internal static string SwitchMarkdownTagsToHtml(string inputString, List<Tag> tagsForChange)
        {
            if (tagsForChange.Count == 0) return inputString;

            var result = new StringBuilder();

            var startedIndex = 0;
            foreach (var tag in tagsForChange)
            {
                for (var i = startedIndex; i < tag.Index; i++)
                    result.Append(inputString[i]);

                result.Append(tag.Type == TagType.Opening
                    ? $"<{MarkdownTransformerToHtml.TagsInfo[tag.Designations].HtmlDesignation}>"
                    : $"</{MarkdownTransformerToHtml.TagsInfo[tag.Designations].HtmlDesignation}>");

                startedIndex = tag.Index + tag.Designations.Length;
            }

            for (var i = tagsForChange.Last().Designations.Length + tagsForChange.Last().Index; i < inputString.Length; i++)
                result.Append(inputString[i]);

            return result.ToString();
        }

        internal static string RemoveEscapeSymbols(string input) => input.Replace(@"\", "");
    }
}
