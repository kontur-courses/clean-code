namespace Markdown
{
    public static class HtmlConverter
    {
        private static readonly Dictionary<TagType, string> HtmlTagNames = new ()
        {
            [TagType.Header] = "h1",
            [TagType.UnorderedList] = "ul",
            [TagType.ListItem] = "li",
            [TagType.Emphasis] = "em",
            [TagType.Strong] = "strong",
        };

        public static string Open(TagType tagType)
        {
            return $"<{HtmlTagNames[tagType]}>";
        }

        public static string Close(TagType tagType)
        {
            return $"</{HtmlTagNames[tagType]}>";
        }

        public static string Wrap(TagType tagType, string content)
        {
            return Open(tagType) + content + Close(tagType);
        }
    }
}
