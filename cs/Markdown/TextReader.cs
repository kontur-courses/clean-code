namespace Markdown;

public class TextReader
{
    private readonly string _text;
    private int _position;
    private Token? _previousToken;
    public Token? Current { get; private set; }

    public TextReader(string text)
    {
        _text = text;
        Current = null;
    }


    public bool ReadNextToken()
    {
        return false;
    }
}