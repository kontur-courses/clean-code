namespace Markdown.Tags;

/// <summary>
/// Интерфейс тега
/// </summary>
public interface ITag
{
    /// <summary>
    /// Тег, который используется в Markdown
    /// </summary>
    string MdTag { get; }

    /// <summary>
    /// Закрывающий тег в Markdown 
    /// </summary>
    string MdClosingTag { get; }

    /// <summary>
    /// Тег в HTML, соответсвующий тегу Markdown
    /// </summary>
    /// <see cref="MdTag"/>
    string HtmlTag { get; }

    /// <summary>
    /// Самозакрывание тега в HTML
    /// </summary>
    bool SelfClosing => false;

    /// <summary>
    /// Запрет на вложение дочерних элементов определенного типа
    /// </summary>
    IReadOnlyCollection<ITag> DisallowedChildren { get; }

    /// <summary>
    /// Получить атрибуты для рендера в HTML
    /// </summary>
    /// <param name="tagContents">Содержание тега</param>
    /// <returns>Строка с атрибутами для вставки в тег</returns>
    static Dictionary<string, string> GetHtmlTadAttributes(string tagContents) => new Dictionary<string, string>() { };

    /// <summary>
    /// Проверка, что тег соответствует переданному тегу
    /// </summary>
    /// <param name="tag">Тег для сравнения</param>
    /// <returns></returns>
    bool Matches(ITag tag);
}