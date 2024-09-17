namespace Markdown.TagClasses.TagModels;

public class TagModel
{
    public string Name { get; private set; }
    public string MarkdownOpening { get; private set; }
    public string MarkdownClosing { get; private set; }
    public bool ShouldHavePair { get; private set; }
    public bool TakePairTag { get; private set; }
    public string HtmlTagOpen { get; private set; }
    public string HtmlTagClose { get; private set; }

    public TagModel(string name, 
        string markdownOpening, 
        string markdownClosing,
        bool shouldHavePair, 
        bool takePairTag,
        string htmlTagOpen, 
        string htmlTagClose)
    {
        Name = name;
        MarkdownOpening = markdownOpening;
        MarkdownClosing = markdownClosing;
        ShouldHavePair = shouldHavePair;
        TakePairTag = takePairTag;
        HtmlTagOpen = htmlTagOpen;
        HtmlTagClose = htmlTagClose;
    }

    public override string ToString()
    {
        return Name;
    }
}