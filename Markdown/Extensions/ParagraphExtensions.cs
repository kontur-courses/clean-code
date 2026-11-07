using Markdown.Paragraphs;
using Markdown.Tokenization;

namespace Markdown.Extensions;

public static class ParagraphExtensions
{
    public static IEnumerable<Token> Tokenize(this Paragraph paragraph)
        => Tokenizer.Tokenize(paragraph);
}
