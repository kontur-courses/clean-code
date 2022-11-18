namespace Markdown
{
    public static class MdTags
    {
        public static readonly Tag Italic = new Tag("_", "_");
        public static readonly Tag Bold = new Tag("__", "__");
        public static readonly Tag Default = new Tag("", "");
        public static readonly Tag Heading = new Tag("# ", "");
    }
}