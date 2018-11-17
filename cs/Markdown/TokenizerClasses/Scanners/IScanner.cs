namespace Markdown.TokenizerClasses.Scanners
{
    public interface IScanner
    {
        bool TryScan(string text, out Token token);
    }
}