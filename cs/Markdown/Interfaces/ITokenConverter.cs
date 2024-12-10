using System.Text;
using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface ITokenConverter
{
    void Render(BaseToken baseToken, StringBuilder result);
}