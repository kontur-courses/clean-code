namespace MarkdownTask
{
    public static class TagInfo
    {
        public enum TagType
        {
            Header,
            Strong,
            Italic,
            Link,
            Empty
        }

        public enum Tag
        {
            Open,
            Close
        }
    }
}