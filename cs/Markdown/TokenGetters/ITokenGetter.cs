using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITokenGetter
    {
        TextToken GetToken(string text, int index, int startPosition);

        bool CanCreateToken(string text, int index, int startPosition);
    }
}