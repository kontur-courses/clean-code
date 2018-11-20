namespace Markdown
{
    public interface IParser
    {
        ITokenNode Parse(string str);
    }
}