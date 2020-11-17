using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tags
    {
        public List<PairTags> PairTags;
        public List<Tag> SingleTags;

        private static List<TagInfo> pairTagsInfo = new List<TagInfo>()
        {
            TagInfo.Em,
            TagInfo.Strong
        };

        private Tags(List<PairTags> pairTags, List<Tag> singleTags)
        {
            PairTags = pairTags;
            SingleTags = singleTags;
        }

        public static Tags BuildTags(string text)
        {
            var allTags = GetAllTags(text).ToList();
            var tagsWithoutShield = GetNotShieldedTags(allTags).ToList();
            var singleTags = GetNotPairTags(tagsWithoutShield).ToList();
            var pairTags = GetPairTags(tagsWithoutShield, Tags.pairTagsInfo).OrderBy(x => x.Start.Position)
                                                                            .ToList();
            var notIncludeTag = GetNotIncludeTag(pairTags, TagInfo.Strong, TagInfo.Em).ToList();
            var correctTags = GetPairTagsWithCorrectSpaces(text, notIncludeTag).ToList();

            return new Tags(correctTags, singleTags);
        }

        private static IEnumerable<PairTags> GetPairTagsWithCorrectSpaces(string text, List<PairTags> pairTags)
        {
            foreach (var pairTag in pairTags)
            {
                if (pairTag.Length == pairTag.Start.Length)
                    continue;

                if (IsTagInsideWord(text, pairTag.Start.Position, pairTag.Start.Length)
                    && char.IsDigit(text[pairTag.Start.Position + pairTag.Start.Length]))
                    continue;

                if (text[pairTag.Start.Position + pairTag.Start.Length] != ' ' && text[pairTag.End.Position - 1] != ' ')
                    yield return pairTag;
                else if (IsTagInsideWord(text, pairTag.Start.Position, pairTag.Start.Length)
                         && text.IndexOf(' ', pairTag.Start.Position, pairTag.Length) == -1)
                    yield return pairTag;
            }
        }

        private static bool IsTagInsideWord(string text, int positions, int length) =>
            text.Length > positions + length
            && positions > 0
            && text[positions + length] != ' '
            && text[positions - 1] != ' ';

        private static IEnumerable<PairTags> GetNotIncludeTag(List<PairTags> pairTags, TagInfo insideTagInfo, TagInfo outTagInfo)
        {
            for (var i = 0; i < pairTags.Count; i++)
            {
                if (pairTags[i].Start.Name == outTagInfo)
                {
                    var pairTag = pairTags[i];
                    yield return pairTags[i];
                    while (i + 1 < pairTags.Count && pairTags[i + 1].Start.Position < pairTag.End.Position)
                    {
                        if (pairTags[i + 1].Start.Name != insideTagInfo)
                            yield return pairTags[i + 1];
                        i++;
                    }
                }
                else
                    yield return pairTags[i];
            }
        }

        private static IEnumerable<Tag> GetNotPairTags(List<Tag> tags) =>
            tags.Where(tag => tag.Name == TagInfo.H1 || tag.Name == TagInfo.Shield || tag.Name == TagInfo.Link);

        private static IEnumerable<PairTags> GetPairTags(List<Tag> tags, List<TagInfo> pairTags)
        {
            var tagStack = new Stack<Tag>();
            var openTags = new HashSet<TagInfo>();
            var ignoreTags = new HashSet<TagInfo>();
            foreach (var tag in tags.Where(tag => pairTags.Contains(tag.Name)))
            {
                if (ignoreTags.Remove(tag.Name))
                    continue;

                if (!openTags.Remove(tag.Name))
                {
                    tagStack.Push(tag);
                    openTags.Add(tag.Name);
                    continue;
                }

                var openTag = tagStack.Pop();
                if (openTag.Name == tag.Name)
                {
                    yield return new PairTags(openTag, tag);
                    continue;
                }

                while (openTag.Name != tag.Name)
                {
                    openTags.Remove(openTag.Name);
                    ignoreTags.Add(openTag.Name);
                    openTag = tagStack.Pop();
                }
            }
        }

        private static IEnumerable<Tag> GetNotShieldedTags(List<Tag> allTags)
        {
            for (var i = 0; i < allTags.Count; i++)
            {
                if (allTags[i].Name == TagInfo.Shield
                    && i + 1 < allTags.Count
                    && allTags[i].Position + 1 == allTags[i + 1].Position)
                {
                    yield return allTags[i];
                    i++;
                }
                else if (allTags[i].Name != TagInfo.Shield)
                    yield return allTags[i];
            }
        }

        private static IEnumerable<Tag> GetAllTags(string paragraph)
        {
            for (var i = 0; i < paragraph.Length; i++)
            {
                var tag = GetTag(i, paragraph);
                if (tag == null)
                    continue;

                i += tag.Length - 1;
                yield return tag;
            }
        }

        private static Tag GetTag(int position, string text)
        {
            switch (text[position])
            {
                case '#':
                    return position == 0
                        ? new Tag(TagInfo.H1, position, 1)
                        : null;
                case '_':
                    if (position + 1 < text.Length && text[position + 1] == '_')
                        return new Tag(TagInfo.Strong, position, 2);
                    else
                        return new Tag(TagInfo.Em, position, 1);
                case '\\':
                    return new Tag(TagInfo.Shield, position, 1);
                case '[':
                    var linkTextEnd = text.IndexOf(']', position);
                    var linkStart = text.IndexOf('(', linkTextEnd);
                    var linkEnd = text.IndexOf(')', linkStart);
                    if (linkTextEnd != -1 && linkStart != -1 && linkEnd != -1 && linkTextEnd + 1 == linkStart)
                        return new Tag(TagInfo.Link, position, linkEnd - position + 1);
                    break;
            }

            return null;
        }
    }
}