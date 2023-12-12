using Markdown.Tokens;

namespace Markdown.Tag;

public class Tag : ITag
{
    public Tag(Token token, TagsState tagState)
    {
        Token = token;
        TagState = tagState;
    }

    public TagsState TagState { get; }
    public Token Token { get; }
}