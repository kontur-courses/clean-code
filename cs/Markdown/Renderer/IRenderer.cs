using Markdown.Tokenizer.Tokens;

namespace Markdown.Renderer;

/// <summary>
/// Универсальный интерфейс рендерера
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Переводит набор токенов в текст языка разметки
    /// </summary>
    /// <param name="tokens">Набор токенов</param>
    /// <returns>Сгенерированный текст</returns>
    string Render(IEnumerable<IToken> tokens);
}