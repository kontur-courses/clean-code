using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            var tokenLength = currentText.Length;
            if (currentText.Length < 3)
                return null;
            if (currentText.ToString().Count(x => x == '_') == currentText.Length)
                return null;
            if (index + 1 >= text.Length && currentText[tokenLength - 1] != '_')
                return null;
            if ((currentText[0] != '_' || index + 1 < text.Length && currentText[tokenLength - 1] != '_') ||
                index + 1 < text.Length && text[index + 1] == '_')
                return null;
            if (currentText.ToString().Any(symbol => symbol == ' ' || char.IsDigit(symbol)))
                return null;
            
            currentText.Remove(0, 1);
            currentText.Remove(currentText.Length - 1, 1);
            var tokenToAdd = new TextToken(index - currentText.Length, currentText.Length,
                TokenType.Emphasized, currentText.ToString());
            var subTokens = new TextParser(tokenGetters).GetTextTokens(tokenToAdd.Text);
            tokenToAdd.SubTokens = subTokens;
            return tokenToAdd;
        }
    }
}