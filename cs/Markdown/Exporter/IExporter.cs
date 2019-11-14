using Markdown.Parser;

namespace Markdown.Exporter
{
    internal interface IExporter
    {
        string TransformText(Text text);

        string TransformBold(MarkdownBoldElement element);

        string TransformItalic(MarkdownItalicElement element);
    }
}