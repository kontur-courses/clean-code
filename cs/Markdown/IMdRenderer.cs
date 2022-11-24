namespace Markdown;

public interface IMdRenderer
{
    string Render(MarkdownParser parser);
}