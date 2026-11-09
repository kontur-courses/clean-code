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

            if (currentChar == '\\' && i + 1 < text.Length && CanEscape(text[i + 1]))
            {
                textBuffer.Append(text[i + 1]);
                i++;
                continue;
            }

            if (currentChar == '\\')
            {
                textBuffer.Append(currentChar);
                continue;
            }

            if (currentChar == '_')
            {
                if (i + 1 < text.Length && text[i + 1] == '_')
                {
                    if (textBuffer.Length > 0)
                    {
                        tokens.Add(new InlineToken(InlineTokenKind.Text, textBuffer.ToString()));
                        textBuffer.Clear();
                    }
                    tokens.Add(new InlineToken(InlineTokenKind.StrongMarker, "__"));
                    i++;
                }
                else
                {
                    if (textBuffer.Length > 0)
                    {
                        tokens.Add(new InlineToken(InlineTokenKind.Text, textBuffer.ToString()));
                        textBuffer.Clear();
                    }
                    tokens.Add(new InlineToken(InlineTokenKind.EmphasisMarker, "_"));
                }

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
        character == '\\' || char.IsPunctuation(character) || char.IsSymbol(character);
}
