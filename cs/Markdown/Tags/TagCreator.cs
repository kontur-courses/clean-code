namespace Markdown.Tags
{
    public static class TagCreator
    {
        public static Dictionary<string, Tag> GetAllSupportedTags()
        {
            return new Dictionary<string, Tag>()
            {
                { "_", new Tag("_", "_", "em")},
                {"__", new Tag("__", "__", "strong")},
                {"# ", new Tag("# ", "\n", "h1")}
            };
        }
    }
}
