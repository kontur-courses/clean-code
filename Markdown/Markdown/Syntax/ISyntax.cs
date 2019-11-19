namespace Markdown
{
    public interface ISyntax
    {
        bool TryGetCharAttribute(string source, int charPosition, out Attribute attribute);
    }
}