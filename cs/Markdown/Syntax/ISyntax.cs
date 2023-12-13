using Markdown.Tag;

namespace Markdown.Syntax;

public interface ISyntax
{
    ITag ConvertTag(TagType type);
}