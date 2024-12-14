namespace Markdown.Token;

public record MdToken(MdTokenType Type, MdTokenBehaviour Behaviour, ReadOnlyMemory<char> Text);