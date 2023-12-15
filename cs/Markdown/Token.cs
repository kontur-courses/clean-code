using System.Text;

namespace Markdown;

public class Token
{
    public bool isSpaces = false;
    public bool isDigitText = false;
    public string Content;
    public TokenStatus Status;
    public readonly string Prev;
    public readonly string Last;
    public Token(string content, TokenStatus tagStatus, string prev = "", string last = "")
    {
        this.Last = last;
        this.Prev = prev; 
        Status = tagStatus;
        this.Content = content;
    }
}