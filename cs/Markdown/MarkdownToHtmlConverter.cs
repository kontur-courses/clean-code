using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MarkdownToHtmlConverter
    {
        private static readonly Dictionary<TagInfo, string> tagToString = new Dictionary<TagInfo, string>()
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
            var linkTags = new Dictionary<int, Tag>();
            var isH1 = notPairTag.Count > 0 && notPairTag[0].Name == TagInfo.H1;
            var shieldPositions = new HashSet<int>();

            foreach (var tag in notPairTag)
            {
                switch (tag.Name)
                {
                    case TagInfo.Shield:
                        shieldPositions.Add(tag.Position);
                        break;
                    case TagInfo.Link:
                        linkTags[tag.Position] = tag;
                        break;
                }
            }

            foreach (var pairTag in pairTags)
            {
                tagsStart[pairTag.Start.Position] = pairTag.Start;
                tagsEnd[pairTag.End.Position] = pairTag.End;
            }

            return Convert(text, isH1, tagsStart, tagsEnd, shieldPositions, linkTags);
        }

        private static string Convert(string text,
                                      bool isH1,
                                      Dictionary<int, Tag> tagsStart,
                                      Dictionary<int, Tag> tagsEnd,
                                      HashSet<int> shieldPositions,
                                      Dictionary<int, Tag> linkTags)
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
                else if (linkTags.TryGetValue(i, out tag))
                {
                    var linkTextEnd = text.IndexOf(']', i);
                    var linkStart = linkTextEnd + 1;
                    var titleStart = text.IndexOf(' ', linkStart);
                    var linkEnd = text.IndexOf(')', linkStart);
                    var linkText = text.Substring(i + 1, linkTextEnd - i - 1);
                    if (titleStart == -1)
                    {
                        var link = text.Substring(linkStart + 1, linkEnd - linkStart - 1);
                        html.Append($"<a href=\"{link}\">{linkText}</a>");
                    }
                    else
                    {
                        var link = text.Substring(linkStart + 1, titleStart - linkStart - 1);
                        var title = text.Substring(titleStart + 2, text.IndexOf('"', titleStart + 2) - titleStart - 2);
                        html.Append($"<a href=\"{link}\" title=\"{title}\">{linkText}</a>");
                    }

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