using Markdown.Token;

namespace Markdown.TagConverter;

public interface IConverter
{
    string ConvertTags(IList<IToken> tokens, string source);
}