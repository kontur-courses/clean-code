using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITokenGetter
    {
        TextToken GetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters, int index,
            string text);

        bool CanCreateToken(StringBuilder currentText, string text, int index);
    }
}