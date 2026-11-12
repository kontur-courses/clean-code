namespace MarkDown;

public class Symbol(SymbolType symbolType, char content, int index)
{
    public SymbolType SymbolType { get; set; } = symbolType;
    public char Contet { get; set; } = content;
    public int Index { get; set; } = index;
}