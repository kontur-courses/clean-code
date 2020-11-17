namespace Markdown
{
    public class Tag
    {
        public bool isCorrect;

        private Tag(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; }
        public string Mark => Marks.GetMarkByHtmlTag(TagName);
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