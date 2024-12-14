using Markdown.NodeView;

namespace Markdown.ParseTree;

public record ParseTreeNodeView<TTokenType>(
    ReadOnlyMemory<char> Text,
    TTokenType TokenType,
    bool Empty,
    bool Complete,
    bool insideWord) : BaseNodeView<TTokenType>;