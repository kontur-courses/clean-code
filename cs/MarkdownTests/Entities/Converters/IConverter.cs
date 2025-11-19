
namespace Markdown.Entities.Renderers
{
    /// <summary>
    /// Интерфейс для класса-конвертера который умеет конвертировать один язык разметки в другой, например конвертер из Markdown в HTML
    /// <param name="text">Исходный текст с определенным языком разметки который хотим преобразовать</param>
    /// </summary>
    public interface IConverter
    {
        string Convert(string text);
    }
}
