namespace Markdown.Tokenization.Tokens;

public class ResolvingContext(PairRole role, int segmentId)
{
    public PairRole Role { get; set; } = role;
    public int SegmentId { get; } = segmentId;
}