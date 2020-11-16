using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITokenGetter
    {
        TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters, int index);

        bool CanCreateToken(StringBuilder currentText, string text, int index);
    }
}