namespace Markdown.Tokenizing;

public class Cursor
{
    private int index;
    private string text;

    public Cursor(string text)
    {
        this.text = text;
    }

    public char CurrentChar => text[index];

    public bool IsEndOfText => index >= text.Length;

    public void MoveForward(int count = 1)
    {
        if (index + count <= text.Length)
            index += count;
    }

    public bool IsNextCharSame(char c)
    {
        return index + 1 < text.Length && text[index + 1] == c;
    }
}