using Markdown.Tokenization.Tokens;

namespace Markdown.Tokenization;

public class Pair(PairToken opening, PairToken closing)
{
    public PairToken Opening { get; } = opening;
    public PairToken Closing { get; } = closing;
    public bool IsInvalid { get; set; }

    public int Start => Opening.Start;
    public int End => Closing.Start + Closing.Length;
}