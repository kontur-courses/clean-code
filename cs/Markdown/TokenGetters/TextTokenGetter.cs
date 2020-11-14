using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TextTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters, int index,string text)
        {
            if (index + 1 < text.Length && (currentText[0] == '_' || text[index + 1] != '_')) return null;
            if (index + 2 == text.Length && currentText[0] != '_' && text[index + 1] == '_') return null;
            var tokenToAdd = new TextToken(index - currentText.Length + 1, currentText.Length, TokenType.Text,
                currentText.ToString());
            return tokenToAdd;

        }
 
    }
}