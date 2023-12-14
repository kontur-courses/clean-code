using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public interface ITextParser<T> where T : IConvertableToString
{
    List<IToken<T>> ParseText(string text);
}