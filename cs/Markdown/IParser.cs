namespace Markdown
{
    public interface IParser<out TToken>
        where TToken : IToken
    {
        TToken[] Parse(string str);
    }
}