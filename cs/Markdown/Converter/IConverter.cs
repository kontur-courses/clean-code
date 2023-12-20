using Markdown.Token;

namespace Markdown.Converter;

public interface IConverter
{
    string ConvertTags(IList<IToken> tokens, string source);
}