using Markdown.Tags;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Rules;

/// <summary>
/// Класс правила парсинга. Содержит функцию проверки валидности тега
/// </summary>
public class Rule : IRule
{
    public Func<TagToken, string, bool, List<TagToken>, bool>? IsValid { get; init; }
    public Func<ITag, string, int, bool>? IsTag { get; init; }
}