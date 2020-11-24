using System;

namespace Markdown.Tags
{
    public class Tag
    {
        public string Value { get; }
        public string HtmlValue { get; }
        public string CompletionSign { get; }
        public TagType TagType { get; }

        public Tag(string value, string htmlValue, string completionSign)
        {
            Value = value;
            HtmlValue = htmlValue;
            CompletionSign = completionSign;
            TagType = GetTagType();
        }

        private TagType GetTagType()
        {
            return Value switch
            {
                "_" => TagType.Italic,
                "__" => TagType.Boldface,
                "# " => TagType.Header,
                _ => throw new ArgumentException("Invalid tag parameter value")
            };
        }
    }
}