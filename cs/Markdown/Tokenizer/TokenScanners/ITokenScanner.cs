using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public interface ITokenScanner
{
    bool TryGetToken(string markdown, int index, out Token? token);
}