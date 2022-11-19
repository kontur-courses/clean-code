namespace Markdown.Tags;

public class Tag
{
    public string Opening { get; }
    public string Closing { get; }
    
    public Tag(string opening, string closing)
    {
        Opening = opening;
        Closing = closing;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Tag other) return false;
        if (ReferenceEquals(this, obj)) return true;

        return Opening == other.Opening && Closing == other.Closing;
    }

    public override int GetHashCode()
    {
        return (Opening, Closing).GetHashCode();
    }
}