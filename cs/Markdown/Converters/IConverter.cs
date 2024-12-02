using Markdown.Tokenizers;

namespace Markdown.Converters;

public interface IConverter
{
    public string Convert(IEnumerable<Token> tokens);
}