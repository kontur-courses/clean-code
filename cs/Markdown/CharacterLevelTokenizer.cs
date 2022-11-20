using System.Collections.Generic;

namespace Markdown
{
    public class CharacterLevelTokenizer
    {
        /*Определение токенов первого уровня из текста: Пробел, нижнее подчёркивание, решётка, обратный слеш, новая строка, а всё остальное - строки
Формируется список, в котором лежат Token: значение строки и её тип, если склеить все строки по порядку, то получится исходный текст.
*/
        public List<FirstTokenType> Tokenize(string inputString)
        {
            return new List<FirstTokenType>();
        }
    }
}