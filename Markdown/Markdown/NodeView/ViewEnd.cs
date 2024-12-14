namespace Markdown.NodeView;

public record ViewEnd<TTokenType>(TTokenType TokenType) : BaseNodeView<TTokenType>;