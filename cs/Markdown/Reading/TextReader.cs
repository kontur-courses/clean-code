namespace Markdown.Reading;

public class TextReader
{
    private readonly string _text;
    private int _position;
    public Token? Current { get; private set; }

    public TextReader(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Text should not be empty");

        _text = text;
        Current = null;
        _position = -1;
    }

    public bool ReadNextToken()
    {
        if (_position == -1)
        {
            Current = new Token('\0', _position, true);
            _position++;
            return true;
        }

        if (_position == _text.Length)
        {
            Current = new Token('\0', _position, true);
            _position++;
            return true;
        }

        if (_position > _text.Length)
        {
            Current = null;
            return false;
        }

        var currentSymbol = _text[_position];
        var currentToken = new Token(currentSymbol, _position);
        Current = currentToken;
        _position++;

        return true;
    }
}