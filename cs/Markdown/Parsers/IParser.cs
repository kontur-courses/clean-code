
namespace Markdown.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// Пытается обработать участок строки, начинающийся с командного символа
        /// </summary>
        /// <param name="position">Позиция, на которой находится командный символ</param>
        /// <param name="text">Строка, в которой происходит парсинг</param>
        /// <returns>Token, если удалось обработать тэг; null, если обработчик его не поддерживает</returns>
        Token TryHandleTag(int position, string text);
    }
}