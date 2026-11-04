using Markdown.Syntax;
using Markdown.Lexer;

namespace Markdown.Parsing;

/// <summary>
/// Разбирает токены разметки и строит дерево узлов
/// </summary>
public class InlineParser()
{
    private readonly InlineLexer inlineLexer = new();

    /// <summary>
    /// Здесь IReadOnlyList для защиты от изменения узлов после их создания
    /// </summary>
    public IReadOnlyList<InlineNode> Parse(string text)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Проверяет, подходит ли маркер на роль открывающего
    /// </summary>
    /// <param name="token">Токен для проверки</param>
    /// <param name="previousToken">Предыдущий токен</param>
    /// <param name="nextToken">Следующий токен</param>
    private bool CanOpen(InlineToken token, InlineToken? previousToken, InlineToken? nextToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Проверяет, подходит ли маркер на роль закрывающего
    /// </summary>
    /// <param name="token">Токен для проверки</param> 
    /// <param name="previousToken">Предыдущий токен</param>
    /// <param name="nextToken">Следующий токен</param>
    private bool CanClose(InlineToken token, InlineToken? previousToken, InlineToken? nextToken)
    {
        throw new NotImplementedException();
    }
}
