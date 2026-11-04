using Markdown.Parsing;

namespace Markdown;

/// <summary>
/// Тут оставил только BlockParser, остальное перенёс в соответствующие классы
/// Хотя не уверен, что это правильно
/// До этого все зависимости были в Md, и можно было увидеть их связь
/// </summary>
public class Md
{
    private readonly BlockParser blockParser;

    public Md()
    {
        blockParser = new BlockParser();
    }

    public string Render(string markdownText)
    {
        throw new NotImplementedException();
    }
}
