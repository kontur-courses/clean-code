using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HeaderTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            text = text.Remove(0, 1);
            var tokenToAdd = new TextToken(0, text.Length,
                TokenType.Header, text);
            tokenToAdd.SubTokens = new TextParser(tokenGetters).GetTextTokens(tokenToAdd.Text);
            return tokenToAdd;
        }

        public bool CanCreateToken(StringBuilder currentText, string text, int index)
        {
            return currentText.Length == 1 && currentText[0] == '#';
        }
    }
}