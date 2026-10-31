using Markdown.Nodes;

namespace Markdown.Transformers;


public record RawParagraph(TextBlockKind Kind, string Content);

public static class ParagraphSplitter
{
    public static IEnumerable<RawParagraph> Split(string raw)
    {
        throw new NotImplementedException();
    }
}