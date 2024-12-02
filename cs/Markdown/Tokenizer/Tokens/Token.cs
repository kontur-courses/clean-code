namespace Markdown.Tokenizer.Tokens;

public record Token(TypeOfToken Type, string Value, int Length)
{
    public Token(TypeOfToken type) 
        : this(type, string.Empty) { }
    
    public Token(TypeOfToken type, string value) 
        : this(type, value, value.Length) { }
    
    public Token(TypeOfToken type, int length) 
        : this(type, string.Empty, length) { }
}