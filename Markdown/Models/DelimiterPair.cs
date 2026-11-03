using Markdown.Models;

namespace Markdown.Models;

// информация о найденной паре открывающего и закрывающего маркеров
internal sealed record DelimiterPair(
    int OpenTokenIndex,
    int CloseTokenIndex,
    InlineTokenType Type
);