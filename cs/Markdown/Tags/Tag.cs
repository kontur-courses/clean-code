namespace Markdown.Tags
{
    public class Tag
    {
        public Tag(string openValue, string closeValue, string htmlValue)
        {
            OpenValue = openValue;
            CloseValue = closeValue;
            HtmlValue = htmlValue;
            TagType = GetTagTypeByOpenValue(openValue);
        }

        public string OpenValue { get; }
        public string CloseValue { get; }
        public string HtmlValue { get; }
        public TagType TagType { get; }

        private TagType GetTagTypeByOpenValue(string openValue)
        {
            return openValue switch
            {
                "_" => TagType.Italic,
                "__" => TagType.Strong,
                "# " => TagType.Header,
                "" => TagType.None,
                _ => throw new ArgumentException($"Can't find tag by this open value: {openValue}"),
            };
        }
    }
}
