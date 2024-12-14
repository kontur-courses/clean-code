namespace Markdown.Tags;
public class EscapeTag : BaseTagHandler
{
    protected override string Symbol => "\\";
    protected override string HtmlTag => "\\";

    private readonly char[] forbiddenChars = ['_', '\\'];

    public override bool IsTagStart(string text, int index) => text[index].ToString() == Symbol;

    private bool CheckTag(string text, int startIndex)
    => (startIndex + 1 < text.Length && forbiddenChars.Contains(text[startIndex + 1]))
    || (startIndex + 3 < text.Length && text.Substring(startIndex + 1, 2) == "# ");

    public override int ProcessTag(ref string text, int startIndex)
    {
        if (CheckTag(text, startIndex))
        {
            text = text.Remove(startIndex, 1);
            if (startIndex + 2 < text.Length && text.Substring(startIndex, 2) == "__")
                return startIndex + 2;
        }
        return startIndex + 1;
    }
}