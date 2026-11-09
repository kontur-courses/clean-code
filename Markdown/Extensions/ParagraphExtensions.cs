using Markdown.Paragraphs;
using Markdown.Tokenization;
using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Extensions;

public static class ParagraphExtensions
{
    public static IEnumerable<Token> Tokenize(this Paragraph paragraph)
        => new Tokenizer().Tokenize(paragraph);
}
