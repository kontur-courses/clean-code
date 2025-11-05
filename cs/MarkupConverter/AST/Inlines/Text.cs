namespace MarkupConverter.AST.Inlines;

public class Text : InlineLeaf
{
    public string Content { get; }

    public Text(string content) => Content = content;
}