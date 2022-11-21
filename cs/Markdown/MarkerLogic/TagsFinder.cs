using System.Text;
using Markdown.Interfaces;
using Markdown.TagClasses;

namespace Markdown.MarkerLogic
{
    public class TagsFinder : ITagsFinder
    {
        public List<TagInfo> CreateTagList(string paragraph)
        {
            var result = new List<TagInfo>();
            var header = GetHeaderInfo(paragraph);
            var sb = new StringBuilder(paragraph);
            if (header is not null)
            {
                result.Add(header);

                if (!header.IsEscaped)
                {
                    sb[0] = ' ';
                }
            }

            paragraph = sb.ToString();
            result.AddRange(FindPairedTags(paragraph, TagType.Strong, "__"));

            var notEscapedStarters = result.Where(x => !x.IsEscaped && x.Type == TagType.Strong).ToList();

            foreach (var tag in notEscapedStarters)
            {
                var atWordBeginning = tag.CanBeStarter && !tag.CanBeEnder;
                var replacement = atWordBeginning ? ' ' : 'a';
                sb[tag.Position] = replacement;
                sb[tag.Position + 1] = replacement;
            }

            paragraph = sb.ToString();
            result.AddRange(FindPairedTags(paragraph, TagType.Emphasis, "_"));
            result.AddRange(FindPictureTags(paragraph));
            RemoveTagsInPictures(result);
            SwitchTagsOrder(result);
            result.Sort();
            return result;
        }


        private static TagInfo? GetHeaderInfo(string paragraph)
        {
            if (!paragraph.Contains('#'))
                return null;

            var lineBeforeTag = paragraph.Split('#')[0];
            if (lineBeforeTag.Length == 0)
            {
                var header = new TagInfo(0, TagType.Header);
                return header;
            }

            if (lineBeforeTag.Length == 1 && IsEscaped(lineBeforeTag))
                return new TagInfo(1, TagType.Header, isEscaped: true);

            return null;
        }

        private static IEnumerable<TagInfo> FindPairedTags(string paragraph, TagType type, string tag)
        {
            var position = 0;
            List<TagInfo> info = new();
            var paragraphShards = paragraph.Split(tag);
            if (paragraphShards.Length == 1)
                return info;
            for (var i = 0; i < paragraphShards.Length - 1; i++)
            {
                position += paragraphShards[i].Length + (i == 0 ? 0 : tag.Length);
                if (IsEscaped(paragraphShards[i]))
                {
                    info.Add(new TagInfo(position, type, isEscaped: true));
                    continue;
                }

                var canBeEnder = !paragraphShards[i].EndsWith(' ') && paragraphShards[i] != string.Empty;
                var canBeStarter = !paragraphShards[i + 1].StartsWith(' ') && paragraphShards[i + 1] != string.Empty;
                if (!canBeStarter && !canBeEnder)
                    continue;

                if (canBeStarter ^ canBeEnder)
                {
                    info.Add(new TagInfo(position, type, 0, canBeStarter, canBeEnder));
                    continue;
                }

                var word = WordOperator.GetWordAtPosition(paragraph, position);
                if (!WordOperator.IsWordContainsDigits(word))
                    info.Add(new TagInfo(position, type, 0, canBeStarter, canBeEnder));
            }

            return info;
        }

        private static IEnumerable<TagInfo> FindPictureTags(string paragraph)
        {
            var result = new List<TagInfo>();
            var shards = paragraph.Split("](");
            var position = 0;
            for (var i = 0; i < shards.Length - 1; i++)
            {
                var haveStart = shards[i].LastIndexOf('[');

                if (haveStart == -1 || shards[i][haveStart - 1] != '!')
                    continue;
                var startingPosition = position + haveStart - 1;


                var haveEnd = shards[i + 1].IndexOf(')');
                if (haveEnd == -1)
                    continue;
                if (IsEscaped(shards[i][..(haveStart - 1)]))
                {
                    result.Add(new TagInfo(startingPosition, TagType.Picture, isEscaped: true));
                    position += shards[i].Length + 2 * (i + 1);
                    continue;
                }

                var end = position + haveEnd + 1;
                var length = end + 2 + (shards[i].Length - startingPosition);
                var content = shards[i + 1][..haveEnd];
                result.Add(new TagInfo(startingPosition, TagType.Picture, length, content: content));
                position += shards[i].Length + 2 * (i + 1);
            }

            return result;
        }

        private static void RemoveTagsInPictures(ICollection<TagInfo> tags)
        {
            var tagsListsPics = tags.Where(x => x.Type == TagType.Picture && !x.IsEscaped)
                .ToList()
                .Select(pic => tags
                    .Where(x => x.Position > pic.Position && x.Position < pic.Position + pic.Length)
                    .ToList());

            foreach (var innerTags in tagsListsPics)
            {
                innerTags.ForEach(x => tags.Remove(x));
            }
        }

        private static void SwitchTagsOrder(IReadOnlyCollection<TagInfo> tags)
        {
            var strongEnders = tags
                .Where(x => !x.IsEscaped && x.CanBeEnder && x.Type == TagType.Strong).ToList();
            var emphasisEnders = tags
                .Where(x => !x.IsEscaped && x.CanBeEnder && !x.CanBeStarter && x.Type == TagType.Emphasis).ToList();
            strongEnders.ForEach(s =>
            {
                var afterStrong = emphasisEnders.FirstOrDefault(e => e.Position == s.Position + 2);
                if (afterStrong is not null)
                {
                    s.Position += 1;
                    afterStrong.Position -= 2;
                    s.CanBeStarter = false;
                }
            });
        }

        private static bool IsEscaped(string shard)
        {
            var lengthBefore = shard.Length;
            var lengthAfter = shard.TrimEnd('\\').Length;
            return (lengthBefore - lengthAfter) % 2 == 1;
        }
    }
}