namespace Markdown;

public class Tag(TagType type, PairTokenType pairType, string text)
{
    public static Tag Create(TagType type, PairTokenType pairType, char ch) => new Tag(type, pairType, ch.ToString());

    public static Tag Create(TagType type, PairTokenType pairType, string str) => new Tag(type, pairType, str);

    public TagType Type { get; set; } = type;
    public PairTokenType PairType { get; set; } = pairType;
    public string TagText { get; set; } = text;
}
