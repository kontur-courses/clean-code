using System.Text;

namespace Markdown.Lexer;

/// <summary>
/// Представляет лексер, который разбивает текст Markdown на токены
/// Чисто помощник, чтобы InlineParser'у было проще строить дерево узлов
/// Экранирование тоже можно здесь обрабатывать
/// </summary>
public sealed class InlineLexer
{
    /// <summary>
    /// Разбивает текст на токены inline-разметки
    /// </summary>
    /// <param name="text">Текст для токенизации</param>
    /// <returns>Список токенов</returns>
    public IReadOnlyList<InlineToken> Tokenize(string text)
    {
        var tokens = new List<InlineToken>();
        var textBuffer = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            var currentChar = text[i];

            if (currentChar == '\\')
            {
                if(i + 1 < text.Length && CanEscape(text[i + 1]))
                {
                    textBuffer.Append(text[i + 1]);
                    i++;
                }
                else
                {
                    textBuffer.Append(currentChar);
                }
                continue;
            }

            if (currentChar == '_')
            {
                var markerLength = 1;
                while (markerLength < 3 &&
                        i + markerLength < text.Length &&
                        text[i + markerLength] == '_')
                {
                    markerLength++;
                }

                if (textBuffer.Length > 0)
                {
                    tokens.Add(new InlineToken(InlineTokenKind.Text, textBuffer.ToString()));
                    textBuffer.Clear();
                }

                var (markerKind, markerValue) = markerLength switch
                {
                    1 => (InlineTokenKind.EmphasisMarker, "_"),
                    2 => (InlineTokenKind.StrongMarker, "__"),
                    _ => (InlineTokenKind.StrongEmphasisMarker, "___")
                };

                tokens.Add(new InlineToken(markerKind, markerValue));
                i += markerLength - 1;
                continue;
            }

            textBuffer.Append(currentChar);
        }

        if (textBuffer.Length > 0)
        {
            tokens.Add(new InlineToken(InlineTokenKind.Text, textBuffer.ToString()));
        }

        return tokens;
    }

    private static bool CanEscape(char character) =>
        character is '\\' or '_' or '#' or '-';
}
