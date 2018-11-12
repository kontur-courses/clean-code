namespace Markdown
{
    public interface ITagTranslator
    {
        string TranslateOpeningTag(string tag);
        string TranslateClosingTag(string tag);
    }
}