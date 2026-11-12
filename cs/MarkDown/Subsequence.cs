namespace MarkDown;

public class Subsequence(int start, int length, bool dead)
{
    public int StartIdx { get; set; } = start;
    public int Length { get; set; } = length;
    
    public bool CanOpen { get; set; }
    public bool CanClose { get; set; }
    public bool IsDead { get; set; } = dead;
    
    public List<Symbol>  _symbols { get; set; }
    
}