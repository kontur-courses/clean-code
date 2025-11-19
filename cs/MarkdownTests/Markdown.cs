using Markdown.Entities.Builders;
using Markdown.Entities.Converters;
using Markdown.Entities.Parsers;

namespace markdown
{
    /// <summary>
    /// Преобразует Markdown-разметку в HTML-код.
    /// Этот класс оставил, хотя в данный момент просто отдает строку преобразованную из Markdown в HTML, но тут можно развить реализацию дальше,
    /// допустим если захотим выгружать текст куда-то или печатать на консоль то можем реализовывать логику обработки вывода здесь
    /// </summary>
    /// <remarks>
    /// Принцип работы:
    /// 1. Исходный текст разбивается на токены (лексемиы) с помощью <see cref="MarkdownTokenizer"/>
    /// 2. Токены преобразуются в абстрактное синтаксическое дерево (AST) класса <see cref="SyntaxTree"/>
    /// 3. Обходом графа AST полученное дерево превращается в текст с HTML с помощью <see cref="HtmlBuilder"/>
    /// </remarks>
    public class Markdown
    {
        public static string Render(string text)
        {
            var markdownConverter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            return markdownConverter.Convert(text);
        }
    }
}