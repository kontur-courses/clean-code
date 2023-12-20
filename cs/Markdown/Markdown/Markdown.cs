using Markdown.Builders;
using Markdown.Parsers;
using Markdown.Tags.TextTag;
using Markdown.TagsMappers;

namespace Markdown;

public static class Markdown
{
    public static string Render(string text, ITextParser<Tag> parser)
    {
        var mapper = new MarkdownToHtmlTagsMapper();
        var builder = new TagTokensBuilder(mapper);

        var textTokens = parser.ParseText(text);

        return builder.Build(textTokens);
    }
} 