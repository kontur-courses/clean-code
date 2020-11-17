using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HeaderTokenGetter : ITokenGetter
    {
        public TextToken GetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            var tokenToAdd = new TextToken(0, text.Length,
                TokenType.Header, text);

            currentText.Clear();
            currentText.Append(text.Remove(0, 1));

            return tokenToAdd;
        }

        public bool CanCreateToken(StringBuilder currentText, string text, int index)
        {
            return currentText.Length == 1 && currentText[0] == '#';
        }
    }
}