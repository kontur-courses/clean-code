using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TextTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            if (index + 1 < text.Length && (currentText[0] == '_' || text[index + 1] != '_')) return null;
            if (index + 2 == text.Length && currentText[0] != '_' && text[index + 1] == '_') return null;
            if (index + 1 < text.Length && currentText[currentText.Length - 1] == '\\' &&
                text[index + 1] == '_') return null;
            var newText = new StringBuilder();
            for (var i = 0; i < currentText.Length; i++)
            {
                if (currentText[i] == '\\' && i + 1 < currentText.Length)
                {
                    if (currentText[i + 1] == '\\' || currentText[i + 1] == '_')
                        i++;
                }

                newText.Append(currentText[i]);
            }

            var tokenToAdd = new TextToken(index - newText.Length + 1, newText.Length, TokenType.Text,
                newText.ToString());
            return tokenToAdd;
        }
    }
}