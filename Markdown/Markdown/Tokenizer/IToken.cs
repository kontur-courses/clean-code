namespace Markdown
{
    public interface IToken
    {
        AttributeType Type { get; }
        int Position { get; }
        int AttributeLength { get; }
    }
}