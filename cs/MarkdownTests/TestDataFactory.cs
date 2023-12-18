using Markdown.Filter.MarkdownFilters;
using Markdown.Lexer;
using Markdown.Tokens.Types;

namespace MarkdownTests;

public static class TestDataFactory
{
    public static MarkdownLexer Lexer => new MarkdownLexerBuilder(new MarkdownFilter())
        .WithTokenType(new EmphasisToken())
        .WithTokenType(new StrongToken())
        .WithTokenType(new HeaderToken())
        .Build();
}