namespace MarkdownRenderer.Abstractions;

public interface IDocumentsConverter
{
    string Convert(string source);
}