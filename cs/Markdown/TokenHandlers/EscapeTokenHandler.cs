using Markdown.Tokens;

namespace Markdown.TokenHandlers;

internal class EscapeTokenHandler : ITokenHandler
{
    public void Handle(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        if (node is { Value: EscapeToken escapeToken, Next: not null })
            escapeToken.Escape(node.Next.Value);
    }
}