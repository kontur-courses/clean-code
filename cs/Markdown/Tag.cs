namespace Markdown
{
    public class Tag
    {
        private Tag(string tagName)
        {
            TagName = tagName;
            TagType = HtmlTags.GetTagType(tagName);
        }

        public bool isCorrect { get; private set; }
        public TagType TagType { get; }
        public string TagName { get; }
        public string Mark => HtmlTags.GetMarkByHtmlTag(TagName);
        public int OpenPosition { get; set; }
        public int ClosePosition { get; private set; }

        public static Tag Correct(string tagName, int start, int end)
        {
            return new Tag(tagName)
            {
                OpenPosition = start,
                ClosePosition = end,
                isCorrect = true
            };
        }

        public static Tag Incorrect(string tagName, int start, int end)
        {
            return new Tag(tagName)
            {
                OpenPosition = start,
                ClosePosition = end,
                isCorrect = false
            };
        }

        public static Tag EmptyOn(int position)
        {
            return new Tag("")
            {
                OpenPosition = position,
                ClosePosition = position
            };
        }
    }
}