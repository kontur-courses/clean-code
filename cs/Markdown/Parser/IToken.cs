namespace Markdown
{
    public interface IToken
    {
        TokenPairType PairType { get; set; }

        string Type { get; set; }

        string Value { get; set; }
    }
}