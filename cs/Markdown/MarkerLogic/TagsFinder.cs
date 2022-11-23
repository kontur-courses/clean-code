using System.Text;
using Markdown.ITagsInterfaces;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.MarkerLogic
{
    public class TagsFinder : ITagsFinder
    {
        private const char FillerSymbol = 'a';
        public List<ITag> CreateTagList(string paragraph)
        {
            var result = new List<ITag>();
            var header = GetHeaderInfo(paragraph);

            if (header is not null)
            {
                result.Add(header);
                paragraph = HideHeader(header, paragraph);
            }

            result.AddRange(FindPairedTags(paragraph, TagType.Strong, "__"));
            paragraph = HideStrongTags(result, paragraph);
            result.AddRange(FindPairedTags(paragraph, TagType.Emphasis, "_"));
            result.AddRange(FindTagsWithInnerText(paragraph, TagType.Picture, '!'));
            RemoveTagsInPictures(result);
            SwitchTagsOrder(result);
            result.Sort();
            return result;
        }

        private static string HideHeader(ITag header, string paragraph)
        {
            var sb = new StringBuilder(paragraph);
            if (header.IsEscaped)
                return paragraph;

            sb[0] = ' ';
            return sb.ToString();
        }

        private static string HideStrongTags(IEnumerable<ITag> tags, string paragraph)
        {
            var sb = new StringBuilder(paragraph);
            var notEscapedStarters = tags.Where(x => !x.IsEscaped && x.Type == TagType.Strong).Select(x => (PairedTag)x)
                .ToList();

            foreach (var tag in notEscapedStarters)
            {
                var atWordBeginning = tag.CanBeStarter && !tag.CanBeEnder;
                var replacement = atWordBeginning ? ' ' : FillerSymbol;
                sb[tag.Position] = replacement;
                sb[tag.Position + 1] = replacement;
            }

            return sb.ToString();
        }

        private static HeaderTag? GetHeaderInfo(string paragraph)
        {
            if (!paragraph.Contains('#'))
                return null;

            var lineBeforeTag = paragraph.Split('#')[0];
            if (lineBeforeTag.Length == 0)
                return new HeaderTag(0, TagType.Header);

            if (lineBeforeTag.Length == 1 && IsEscaped(lineBeforeTag))
                return new HeaderTag(1, TagType.Header, isEscaped: true);

            return null;
        }

        private static IEnumerable<PairedTag> FindPairedTags(string paragraph, TagType type, string tag)
        {
            var position = 0;
            var paragraphShards = paragraph.Split(tag);
            if (paragraphShards.Length == 1)
                yield break;
            for (var i = 0; i < paragraphShards.Length - 1; i++)
            {
                position += paragraphShards[i].Length + (i == 0 ? 0 : tag.Length);
                if (IsEscaped(paragraphShards[i]))
                {
                    yield return new PairedTag(position, type, isEscaped: true);
                    continue;
                }

                var canBeEnder = !paragraphShards[i].EndsWith(' ') && paragraphShards[i] != string.Empty;
                var canBeStarter = !paragraphShards[i + 1].StartsWith(' ') && paragraphShards[i + 1] != string.Empty;
                if (!canBeStarter && !canBeEnder)
                    continue;

                if (canBeStarter ^ canBeEnder)
                {
                    yield return new PairedTag(position, type, canBeStarter, canBeEnder);
                    continue;
                }

                var word = WordOperator.GetWordAtPosition(paragraph, position);
                if (!WordOperator.IsWordContainsDigits(word))
                    yield return new PairedTag(position, type, canBeStarter, canBeEnder);
            }
        }

        private static IEnumerable<ITag> FindTagsWithInnerText(string paragraph, TagType type, char startingChar)
        {
            var shards = paragraph.Split("](");
            var position = 0;
            for (var i = 0; i < shards.Length - 1; i++)
            {
                var startPosition = shards[i].LastIndexOf('[');

                if (startPosition == -1 || shards[i][startPosition - 1] != startingChar)
                    continue;
                var startingPosition = position + startPosition - 1;


                var haveEnd = shards[i + 1].IndexOf(')');
                if (haveEnd == -1)
                    continue;
                if (IsEscaped(shards[i][..(startPosition - 1)]))
                {
                    yield return new TextTag(startingPosition, type, 0, isEscaped: true);
                    position += shards[i].Length + 2 * (i + 1);
                    continue;
                }

                var end = position + haveEnd + 1;
                var length = end + 2 + (shards[i].Length - startingPosition);
                var content = shards[i + 1][..haveEnd];
                yield return new TextTag(startingPosition, type, length, content: content);
                position += shards[i].Length + 2 * (i + 1);
            }
        }

        private static void RemoveTagsInPictures(ICollection<ITag> tags)
        {
            var tagsListsPics = tags.Where(x => x.Type == TagType.Picture && !x.IsEscaped)
                .OfType<TextTag>()
                .ToList()
                .Select(pic => tags
                    .Where(x => x.Position > pic.Position && x.Position < pic.Position + pic.Length)
                    .ToList());

            foreach (var innerTags in tagsListsPics)
            {
                innerTags.ForEach(x => tags.Remove(x));
            }
        }

        private static void SwitchTagsOrder(IReadOnlyCollection<ITag> tags)
        {
            var strongEnders = tags
                .OfType<PairedTag>()
                .Where(x => !x.IsEscaped && x.CanBeEnder && x.Type == TagType.Strong).ToList();
            var emphasisEnders = tags
                .OfType<PairedTag>()
                .Where(x => !x.IsEscaped && x.CanBeEnder && !x.CanBeStarter && x.Type == TagType.Emphasis).ToList();
            strongEnders.ForEach(s =>
            {
                var afterStrong = emphasisEnders.FirstOrDefault(e => e.Position == s.Position + 2);
                if (afterStrong is null) return;
                s.Position += 1;
                afterStrong.Position -= 2;
                s.CanBeStarter = false;
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