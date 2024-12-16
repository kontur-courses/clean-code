using Markdown.Tags;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Rules;

/// <summary>
/// Интерфейс правила парсинга
/// </summary>
public interface IRule
{
    Func<TagToken, string, bool, List<TagToken>, bool> IsValid { get; init; }
    Func<ITag, string, int, bool>? IsTag { get; init; }
}