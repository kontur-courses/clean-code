using System.Text;

namespace Markdown;

public class HtmlRenderer
{
    // Добавляет текстовое содержимое в HTML-буфер
    public void AppendText(
        StringBuilder output,
        string text)
    {
        throw new NotImplementedException();
    }

    // Оборачивает содержимое в тег
    public void AppendTag(
        StringBuilder output,
        string tag,
        string content)
    {
        throw new NotImplementedException();
    }

    // Оборачивает содержимое в заголовок
    public void AppendHeader(
        StringBuilder output,
        int level,
        string content)
    {
        throw new NotImplementedException();
    }
}
