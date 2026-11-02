using System.Text;

namespace Markdown;

public class StringFormatter
{
    public readonly Dictionary<string, string> PairReplacements;
    public readonly Dictionary<string, string> SingleReplacements;
    public readonly StringBuilder Text;
    public readonly char EscapeSymbol;

    public StringBuilder Format()
    {
        
    }

    private int FindPairSymbolIndex(int fromIndex, int symbolLength)
    {
        
    }

    // Будет определять, что нужно использовать: ReplaceSingleSymbol или ReplacePairSymbols
    private void ReplaceSymbols(int fromIndex, int toIndex, int symbolLength)
    {

    }

    private void ReplaceSingleSymbol(int fromIndex, int toIndex, int symbolLength)
    {
        
    }

    private void ReplacePairSymbols(int fromIndex, int toIndex, int symbolLength)
    {

    }
}