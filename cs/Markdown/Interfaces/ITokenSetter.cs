using System.Text;
using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface ITokenSetter<T>
    where T : Enum
{
    public void SetToken(List<Token> tokens, T type, ref int index, string line, StringBuilder builder);
}