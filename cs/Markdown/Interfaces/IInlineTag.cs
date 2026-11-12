namespace Markdown.Interfaces;

public interface IInlineTag
{
    string Marker { get; }

    string RenderOpen();

    string RenderClose();

    bool CanContain(IInlineTag inner);
}