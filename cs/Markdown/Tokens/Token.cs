namespace Markdown.Tokens;

public abstract class Token
{
    public abstract string? TagWrapper { get; }
    public abstract bool IsTag { get; }
    public bool IsClosing { get; set; }
    public int PairedTokenIndex { get; set; }
    public string Content { get; }

    protected Token(string content)
    {
        Content = content;
    }

    protected Token(Token token)
    {
        Content = token.Content;
        IsClosing = token.IsClosing;
        PairedTokenIndex = token.PairedTokenIndex;
    }
    
    public abstract bool Validate(List<Token> tokens, int index);
    public abstract bool IsOpen(List<Token> tokens, int index);
}