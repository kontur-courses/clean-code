using System.Text;

namespace Markdown.Parsing.Parsers.InlineParser;

public class Lexer
{
    public List<Token> Tokenize(string text)
    {
        var tokens = new List<Token>();
        var currentText = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c == '\\' && i + 1 < text.Length && TryEscape(text[i + 1]) is char escapedChar)
            {
                currentText.Append(escapedChar);
                i += 1;
                continue;
            }

            if (c == '_')
            {
                var isDouble = (i + 1 < text.Length) && (text[i + 1] == '_');
                var isTriple = isDouble && (i + 2 < text.Length) && (text[i + 2] == '_');
                var length = isTriple ? 3 : (isDouble ? 2 : 1);

                if (!IsValidMarkerContext(text, i, length))
                {
                    currentText.Append(new string('_', length));
                    i += length - 1;
                    continue;
                }

                FlushText(tokens, currentText, i);

                if (isTriple)
                {
                    var rightIsBoundary = (i + 3 >= text.Length) || char.IsWhiteSpace(text[i + 3]);
                    if (rightIsBoundary)
                    {
                        tokens.Add(new Token(TokenType.Italic, "_", i));
                        tokens.Add(new Token(TokenType.Bold, "__", i + 1));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Bold, "__", i));
                        tokens.Add(new Token(TokenType.Italic, "_", i + 2));
                    }
                }
                else if (isDouble)
                {
                    tokens.Add(new Token(TokenType.Bold, "__", i));
                }
                else
                {
                    tokens.Add(new Token(TokenType.Italic, "_", i));
                }

                i += length - 1;
                continue;
            }

            currentText.Append(c);
        }

        FlushText(tokens, currentText, text.Length);
        return tokens;
    }

    private void FlushText(List<Token> tokens, StringBuilder currentText, int currentIndex)
    {
        if (currentText.Length > 0)
        {
            tokens.Add(new Token(TokenType.Text, currentText.ToString(), currentIndex - currentText.Length));
            currentText.Clear();
        }
    }

    private char? TryEscape(char nextChar)
    {
        return nextChar switch
        {
            '_' => '_',
            '\\' => '\\',
            _ => null
        };
    }

    private bool IsValidMarkerContext(string text, int index, int length)
    {
        if (index > 0 && text[index - 1] == '_') return false;

        var left = index > 0 ? text[index - 1] : (char?)null;
        var right = index + length < text.Length ? text[index + length] : (char?)null;

        var leftIsDigit = left.HasValue && char.IsDigit(left.Value);
        var rightIsDigit = right.HasValue && char.IsDigit(right.Value);
        var leftIsLetter = left.HasValue && char.IsLetter(left.Value);
        var rightIsLetter = right.HasValue && char.IsLetter(right.Value);

        if (leftIsDigit && rightIsDigit) return false;
        if ((leftIsDigit && rightIsLetter) || (leftIsLetter && rightIsDigit)) return false;

        var leftIsSpace = !left.HasValue || char.IsWhiteSpace(left.Value);
        var rightIsSpace = !right.HasValue || char.IsWhiteSpace(right.Value);

        return !(leftIsSpace && rightIsSpace);
    }
}