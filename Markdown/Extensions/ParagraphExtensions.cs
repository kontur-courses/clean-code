using Markdown.Paragraphs;
using Markdown.Tokenization;
using Markdown.Tokenization.Tokens;

namespace Markdown.Extensions;

public static class ParagraphExtensions
{
    public static UnresolvedTokens Tokenize(this Paragraph paragraph)
        => new Tokenizer().Tokenize(paragraph);
}
