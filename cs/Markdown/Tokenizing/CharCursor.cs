namespace Markdown.Tokenizing;

public class CharCursor(string text)
{
    private int index;

    public char Current => End ? '\0' : text[index];

    public bool End => index >= text.Length;

    public void MoveNext()
    {
        if (!End)
            index++;
    }

    public bool MatchNext(char c)
    {
        var next = index + 1;
        return (next < text.Length ? text[next] : '\0') == c;
    }
}