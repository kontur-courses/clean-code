using Markdown.NodeView;

namespace Markdown.AbstractSyntaxTree;

public record AbstractSyntaxTreeNodeView<TTokenType>(
    ReadOnlyMemory<char> Text,
    TTokenType TokenType)
    : BaseNodeView<TTokenType>;