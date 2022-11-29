using Markdown.MarkdownDocument;

namespace Markdown.Render;

public interface IMdRenderer
{
    string Render(MdParsedObjectModel objectModel);
}