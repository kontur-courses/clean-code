using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer;
public interface IMdTokenizer
{
    TokenList Tokenize(string text);
}