using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class StrongTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            var tokenLength = currentText.Length;
            if (currentText.Length < 5)
                return null;
            if (currentText.ToString().Count(x => x == '_') == currentText.Length)
                return null;
            if (currentText[0] != '_' || currentText[1] != '_' || currentText[tokenLength - 2] != '_' ||
                currentText[tokenLength - 1] != '_') return null;
            if (currentText.ToString().Any(symbol => symbol == ' ' || char.IsDigit(symbol)))
                return null;
            currentText.Remove(0, 2);
            currentText.Remove(currentText.Length - 2, 2);
            var tokenToAdd = new TextToken(index - currentText.Length - 1, currentText.Length,
                TokenType.Strong, currentText.ToString());
            tokenToAdd.SubTokens = new TextParser(tokenGetters).GetTextTokens(tokenToAdd.Text);
            return tokenToAdd;
        }
    }
}