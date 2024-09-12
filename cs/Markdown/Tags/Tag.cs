namespace Markdown.Tags;

public class Tag
{
    public Tag(string value, string htmlValue, bool isOpen = true)
    {
        Value = value;
        HtmlValue = htmlValue;
        TagType = GetTagTypeByOpenValue(value);
        IsOpen = isOpen;
    }
    
    public bool IsOpen { get; set; }
    public string Value { get; }
    public string HtmlValue { get; }
    public TagType TagType { get; }

    public string CreateHtmlTag(bool isOpen)
    {
        return isOpen ? $"<{HtmlValue}>" : $"</{HtmlValue}>";
    }

    private TagType GetTagTypeByOpenValue(string openValue)
    {
        return openValue switch
        {
            "_" => TagType.Italic,
            "__" => TagType.Strong,
            "# " => TagType.Header,
            "\n" => TagType.Header,
            "[" => TagType.LinkDescription,
            "]" => TagType.LinkDescription,
            "(" => TagType.Link,
            ")" => TagType.Link,
            "\\" => TagType.Escape,
            _ => throw new ArgumentException($"Can't find tag by this open value: {openValue}"),
        };
    }
}
