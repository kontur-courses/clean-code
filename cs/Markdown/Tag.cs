namespace Markdown;

public class Tag
{
    public readonly string Symbol;
    public readonly int Position;
    public readonly bool IsPaired;
    public int Lenght => Symbol.Length;

    public Tag(string symbol, int position, bool isPaired = true)
    {
        Symbol = symbol;
        Position = position;
        IsPaired = isPaired;
    }

    public string BuildHtmlTag(bool isOpenTag)
    {
        var closeMark = isOpenTag ? "" : "/";
        return Symbol == "\\" ? "" : $"<{closeMark}{TagConverter.ConvertMdToHtml(Symbol)}>";
    }
}