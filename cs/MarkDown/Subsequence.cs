namespace MarkDown;

public class Subsequence
{
    private int _start { get; set; }
    private int _length { get; set; }
    
    private bool _canOpen { get; set; }
    private bool _canClose { get; set; }
    private bool _dead { get; set; }
    
    private List<Symbol>  _symbols { get; set; }

    public Subsequence(int start, int length, bool canOpen, bool canClose, bool dead, List<Symbol> symbols)
    {
        _start = start;
        _symbols = symbols;
        
    }
}