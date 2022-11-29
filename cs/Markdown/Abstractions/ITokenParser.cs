using Markdown.Primitives;

namespace Markdown.Abstractions;

public interface ITokenParser
{
    TagNode Parse();
}