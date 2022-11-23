using Markdown.ITagsInterfaces;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.MarkerLogic
{
    public class TagsFilter : ITagsFilter
    {
        public List<ITag> FilterTags(List<ITag> tags, string paragraph)
        {
            var result = new List<ITag>();
            result.AddRange(tags.Where(x => x.IsEscaped || x is not PairedTag).ToList());

            var pairedTags = tags.OfType<PairedTag>()
                .ToList();

            var start = pairedTags
                .FirstOrDefault(x => x.CanBeStarter);
            if (start is null)
                return result;
            while (start is not null)
            {
                var isInvalidStartPoint = !TryFindFirstValidPair(pairedTags, paragraph, start, out var end)
                                          || start.InMiddle() && WordOperator.InDifferentWords(paragraph, start, end);
                if (isInvalidStartPoint)
                {
                    pairedTags.Remove(start);
                    start = pairedTags.FirstOrDefault(x => x.CanBeStarter
                                                           && !x.InPair
                                                           && !x.IsEscaped);
                    continue;
                }

                end.InPair = true;
                if (!HaveIntersects(pairedTags, paragraph, start, end, result))
                {
                    start.CanBeEnder = false;
                    end.CanBeStarter = false;
                    result.Add(start);
                    result.Add(end);
                }

                tags.Remove(start);
                tags.Remove(end);

                start = tags.OfType<PairedTag>()
                    .FirstOrDefault(x => x.CanBeStarter
                                         && !x.InPair
                                         && !x.IsEscaped);
            }


            return result;
        }

        private static bool HaveIntersects(List<PairedTag> tags, string paragraph, PairedTag start, PairedTag end,
            ICollection<ITag> result)
        {
            var results = false;

            var innerStarters = tags.Where(x => x.Position > start.Position && x.Position < end.Position)
                .Where(x => x.CanBeStarter && !x.InPair && !x.IsEscaped)
                .ToList();

            innerStarters.Where(x => !x.CanBeEnder)
                .ToList()
                .ForEach(x => x.InPair = true);

            foreach (var innerStart in innerStarters)
            {
                innerStart.InPair = true;
                var isIntersectLocal = false;

                var isInvalidStartPoint = !TryFindFirstValidPair(tags, paragraph, innerStart, out var innerEnd)
                                          || start.InMiddle()
                                          && WordOperator.InDifferentWords(paragraph, innerStart, innerEnd);
                if (isInvalidStartPoint)
                {
                    tags.Remove(innerStart);
                    continue;
                }

                innerEnd.InPair = true;
                if (innerEnd.Position > end.Position && start.Type != innerStart.Type)
                {
                    isIntersectLocal = true;
                    results = results || isIntersectLocal;
                }

                if (start.Type == TagType.Emphasis || HaveIntersects(tags, paragraph, innerStart, innerEnd, result) ||
                    isIntersectLocal)
                {
                    tags.Remove(innerStart);
                    tags.Remove(innerEnd);
                    continue;
                }

                innerStart.CanBeEnder = false;
                innerEnd.CanBeStarter = false;
                result.Add(innerStart);
                result.Add(innerEnd);
            }

            return results;
        }

        private static bool TryFindFirstValidPair(IReadOnlyCollection<PairedTag> tags, string paragraph,
            PairedTag start, out PairedTag end)
        {
            end = new PairedTag();
            var endTags = tags.Where(x => x.CanBeEnder
                                          && x.Position > start.Position
                                          && x.Type == start.Type
                                          && !x.InPair
                                          && !x.IsEscaped)
                .ToList();
            if (endTags.Count == 0)
                return false;
            var possibleEnders = endTags.Where(possibleEnd =>
                !possibleEnd.InMiddle() || !WordOperator.InDifferentWords(paragraph, start, possibleEnd));

            foreach (var possibleEnd in possibleEnders)
            {
                if (possibleEnd.InMiddle() && possibleEnd.Type == TagType.Emphasis)
                    TrySwitchPosition(tags, possibleEnd);

                end = possibleEnd;
                return true;
            }

            return false;
        }

        private static void TrySwitchPosition(IEnumerable<PairedTag> tags, PairedTag tag)
        {
            var competitiveStrong = tags.FirstOrDefault(x => x.Type == TagType.Strong
                                                             && x.InMiddle()
                                                             && x.Position == tag.Position - 2);
            if (competitiveStrong is null)
                return;
            competitiveStrong.Position += 1;
            tag.Position -= 2;
        }
    }
}