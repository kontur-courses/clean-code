using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public class NewLineTag : Tag
{
    public NewLineTag(string newLineSymbol) : base(new NewLineModel(newLineSymbol))
    {
    }

    public override bool CanBeOpened(string markdownText, int startIndex)
    {
        return false;
    }

    public override bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        return true;
    }
}