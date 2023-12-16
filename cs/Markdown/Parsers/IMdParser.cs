using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IMdParser
    {
        MdDoc Parse(string input);
    }
}