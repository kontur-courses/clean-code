using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md2HtmlTranslator
    {
        public string TranslateMdToHtml(string mdText, Dictionary<MarkdownToken, List<TokenPosition>> markdownTokens)
        {
            var sortedPositions = GetSortedPositionsWithTags(markdownTokens);
            return GetHtmlText(mdText, sortedPositions);
        }

        private string GetHtmlText(string mdText, SortedDictionary<int, Tuple<string,string>> sortedPositionsWithTags)
        {
            var htmlBuilder = new StringBuilder();
            var mdIndex = 0;

            while (mdIndex < mdText.Length)
            {
                if (!sortedPositionsWithTags.ContainsKey(mdIndex))
                {
                    htmlBuilder.Append(mdText[mdIndex]);
                    mdIndex++;
                }
                else
                {
                    var currentPair = sortedPositionsWithTags[mdIndex];
                    htmlBuilder.Append(currentPair.Item1);
                    mdIndex += currentPair.Item2.Length;
                }
            }

            return htmlBuilder.ToString();
        }

        private SortedDictionary<int, Tuple<string,string>> GetSortedPositionsWithTags(Dictionary<MarkdownToken, List<TokenPosition>> markdownTokens)
        {
            var sortedPositionsWithTags = new SortedDictionary<int, Tuple<string, string>>();
            foreach (var tokenWithPositions in markdownTokens)
                foreach (var position in tokenWithPositions.Value)
                {
                    sortedPositionsWithTags.Add(
                        position.Start,
                        new Tuple<string, string>(
                            $"<{tokenWithPositions.Key.HtmlTag}>",
                            tokenWithPositions.Key.Template));
                    sortedPositionsWithTags.Add(
                        position.End,
                        new Tuple<string, string>(
                            $"</{tokenWithPositions.Key.HtmlTag}>",
                            tokenWithPositions.Key.Template));
                }

            return sortedPositionsWithTags;
        }
    }
}
