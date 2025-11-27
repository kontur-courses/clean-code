using System.Text;
using Markdown.Parsing;
using Markdown.Tokenizing;

namespace Markdown;

public class Md
{
    /// <summary>
    /// Возвращает HTML-разметку
    /// </summary>
    /// <param name="markdown">Разметка Markdown для преобразования в HTML</param>
    /// <returns></returns>
    public string Render(string markdown)
    {
        var result = new StringBuilder();
        var tokenizer = new Tokenizer();
        var parser = new NodeParser();
        var lines = markdown.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var tokens = tokenizer.Tokenize(line);
            var nodes = parser.ParseLine(tokens);
            result.AppendLine(nodes.ToHtml());
        }

        return result.ToString();
    }
}