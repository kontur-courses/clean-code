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
    /// Здесь IReadOnlyList для защиты от изменения токенов после их создания
    /// </summary>
    public IReadOnlyList<InlineToken> Tokenize(string text)
    {
        throw new NotImplementedException();
    }
}
