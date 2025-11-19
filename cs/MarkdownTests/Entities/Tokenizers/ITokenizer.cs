using Markdown.Models;

namespace Markdown.Entities.Parsers
{
    /// <summary>
    /// Интерфейс для класса который будет заниматься преобразованием исходного текста в список токенов для дальнейшего построения синтаксического дерева.
    /// Умеет преобразовывать как целый текст в набор токенов, так и обрабатывать коллекции отдельных строк превращая их в список токенов
    /// </summary>
    public interface ITokenizer
    {
        List<Token> Tokenize(string text);

        List<string> TextToLines(string text);

        List<Token> TokenizeLines(IEnumerable<string> lines);

        List<Token> TokenizeLine(string line);
    }
}
