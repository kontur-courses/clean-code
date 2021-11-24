using System.Text;

namespace Markdown.Parsers
{
    public interface IParser
    {
        IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line);
    }
}
