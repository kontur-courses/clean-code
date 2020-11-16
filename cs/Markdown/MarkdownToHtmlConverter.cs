using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MarkdownToHtmlConverter
    {
        private static Dictionary<TagInfo, string> tagToString = new Dictionary<TagInfo, string>()
        {
            {TagInfo.Em, "em"},
            {TagInfo.Strong, "strong"},
            {TagInfo.Shield, ""},
            {TagInfo.H1, "h1"}
        };

        public static string Convert(string text, List<PairTags> pairTags, List<Tag> notPairTag)
        {
            var tagsStart = new Dictionary<int, Tag>();
            var tagsEnd = new Dictionary<int, Tag>();
            var isH1 = notPairTag.Count > 0 && notPairTag[0].Name == TagInfo.H1;
            var shieldPositions = new HashSet<int>();

            foreach (var tag in notPairTag.Where(x => x.Name == TagInfo.Shield))
                shieldPositions.Add(tag.Position);

            foreach (var pairTag in pairTags)
            {
                tagsStart[pairTag.Start.Position] = pairTag.Start;
                tagsEnd[pairTag.End.Position] = pairTag.End;
            }

            return Convert(text, isH1, tagsStart, tagsEnd, shieldPositions);
        }

        private static string Convert(string text,
                                      bool isH1,
                                      Dictionary<int, Tag> tagsStart,
                                      Dictionary<int, Tag> tagsEnd,
                                      HashSet<int> shieldPositions)
        {
            var html = isH1 ? new StringBuilder("<h1>") : new StringBuilder();
            for (var i = isH1 ? 1 : 0; i < text.Length; i++)
            {
                if (tagsStart.TryGetValue(i, out var tag))
                {
                    html.Append($"<{tagToString[tag.Name]}>");
                    i += tag.Length - 1;
                }
                else if (tagsEnd.TryGetValue(i, out tag))
                {
                    html.Append($"</{tagToString[tag.Name]}>");
                    i += tag.Length - 1;
                }
                else if (!shieldPositions.Contains(i))
                    html.Append(text[i]);
            }

            if (isH1)
                html.Append("</h1>");
            return html.ToString();
        }
    }
}