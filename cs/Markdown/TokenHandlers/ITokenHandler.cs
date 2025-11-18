using Markdown.Tokens;

namespace Markdown.TokenHandlers;

internal interface ITokenHandler
{
    void Handle(LinkedListNode<IToken> node, Stack<TagToken> stack);
}