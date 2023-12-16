namespace Markdown;

public class Token
{
    public readonly string Last;
    public readonly string Prev;
    public string Content;
    public bool isDigitText = false;
    public bool isSpaces = false;
    public TokenStatus Status;

    public Token(string content, TokenStatus tagStatus, string prev = "", string last = "")
    {
        Last = last;
        Prev = prev;
        Status = tagStatus;
        Content = content;
    }
}