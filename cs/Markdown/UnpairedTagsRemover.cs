namespace Markdown
{
    public class UnpairedTagsRemover
    {
        public List<TagInfo> FilterTags(List<TagInfo> tags, string paragraph)
        {
            var result = new List<TagInfo>();
            result.AddRange(tags.Where(x => x.IsEscaped || x.type == TagType.Header).ToList());
            var start = tags.FirstOrDefault(x => x.canBeStarter);
            if (start is null)
                return result;
            while (start is not null)
            {
                var isInvalidStartPoint = !TryFindFirstValidPair(tags, paragraph, start, out var end)
                                        || (InDifferentWords(paragraph, start, end) && start.InMiddle());
                if (isInvalidStartPoint)
                {
                    tags.Remove(start);
                    start = // new start tag
                    continue;
                }

                end.InPair = true;
                if (!HaveIntersects(tags, paragraph, start, end, result))
                {
                    //add pair to result
                }


                start = // new start tag
            }

            return result;
        }

        public bool HaveIntersects(List<TagInfo> tags, string paragraph, TagInfo start, TagInfo end,
            List<TagInfo> result)
        {
            var results = false;
            var innerStarters = tags.Where()
                .ToList();
            foreach (var innerStart in innerStarters)
            {
                var isIntersectLocal = false;
                var isInvalidStartPoint = !TryFindFirstValidPair(tags, paragraph, innerStart, out var innerEnd)
                                          || (InDifferentWords(paragraph, innerStart, innerEnd) && innerStart.InMiddle());
                if (isInvalidStartPoint)
                {
                    continue;
                }

                innerEnd.InPair = true;
                if (innerEnd.position > end.position && start.type != innerStart.type)
                {
                }

                if (start.type == TagType.Emphasis || HaveIntersects(tags, paragraph, innerStart, innerEnd, result) ||
                    isIntersectLocal)
                {
                    continue;
                }
            }

            return results;
        }

        public bool TryFindFirstValidPair(List<TagInfo> tags, string paragraph, TagInfo start, out TagInfo end)
        {
            end = new TagInfo();
            var endTags = tags
                .Where()
                .ToList();
            if (endTags.Count == 0)
                return false;
            foreach (var possibleEnd in endTags)
            {
                if (possibleEnd.InMiddle() && InDifferentWords(paragraph, start, possibleEnd))
                    continue;

                end = possibleEnd;
                return true;
            }

            return false;
        }

        public bool InDifferentWords(string paragraph, TagInfo start, TagInfo end)
        {
            return true;
        }
    }
}