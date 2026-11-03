namespace MarkDown;

public class Symbol
{
    private string _type { get; set; } // char или special
    private string _symbol { get; set; }
    private int _index { get; set; }

    public Symbol(string content, string symbol, int index)
    {
        _type = content;
        _symbol = symbol;
        _index = index;
    }
    
}