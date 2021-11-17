using Markdown.TagStore;
using Markdown.Tokens;

namespace Markdown
{
    /*
     Минимальные требования (на 1 балл)
    1. Поддерживаются тэги _, __ и # согласно спецификации
    2. Тесты
    
    Полное решение (на 2 балла)
    1. Выполнены минимальные требования
    2. Решение разбито на составные части, каждая из которых легко читается
    */
    public class Md
    {
        public string Render(string text)
        {
            var converter = new Converter(new MdTagStore(), new HtmlTagStore());
            var converted = converter.Convert(text);
            return converted;
        }
    }
}