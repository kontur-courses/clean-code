using Markdown.Tokens;

namespace Markdown.Tag;

public interface ITag
{
    public TagsState TagState { get; }
    public Token Token { get; }
}