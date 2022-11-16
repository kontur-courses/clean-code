namespace Markdown
{
    public class TagFider
    {
        public List<TagInfo> CreateTagList(string paragraph)
        {
            var result = new List<TagInfo>();
            if (HasHeader(paragraph, out var header))
                result.Add(header);
            result.AddRange(FindPairedTags(paragraph, TagType.Strong, "__"));
            result.AddRange(FindPairedTags(paragraph, TagType.Emphasis, "_"));
            result.Sort();
            return result;
        }

        public bool HasHeader(string paragraph, out TagInfo header)
        {
            header = new(0, TagType.Header);
            if (!paragraph.Contains('#'))
                return false;

            var lineBeforeTag = paragraph.Split('#')[0];

            if (!IsEscaped(lineBeforeTag))
                return false;

            return true;
        }

        public List<TagInfo> FindPairedTags(string paragraph, TagType type, string tag)
        {
            var position = 0;
            List<TagInfo> info = new();
            var paragraphShards = paragraph.Split(tag);
            for (var i = 0; i < paragraphShards.Length - 1; i++)
            {
                position += ;
                if (IsEscaped(paragraphShards[i]))
                {
                    //info.Add(posistion, escaperd = true);
                    continue;
                }

                var canBeEnder = ;
                var canBeStarter = ;
                if (!canBeStarter && !canBeEnder) //nonvalid
                    continue;

                if (canBeStarter ^ canBeEnder)
                {

                    continue;
                }

                var word = GetWordAtPosition(paragraph, position);
                if (!IsWordContainsDigits(word))
                    //info.Add(new TagInfo(position, type, canBeStarter, canBeEnder));
            }

            return info;
        }

        public bool IsEscaped(string shard)
        {
            return true;
        }

        public string GetWordAtPosition(string paragraph, int position)
        {
            return "";
        }

        public bool IsWordContainsDigits(string word)
        {
            return true;
        }
    }
}