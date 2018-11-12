namespace Markdown
{
    public interface IMdParser
    {
        Token[] Parse(string str);
    }
}