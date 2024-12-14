using Markdown.Converters;
using Markdown.Parsers.MarkdownLineParsers;
using Markdown.Parsers.TokensParsers;

namespace Markdown;

public class Md
{
    private readonly IMarkdownLineParser markdownLineParser;
    private readonly ITokensParser tokensParser;
    private readonly IHtmlConverter htmlConverter;

    public Md(IMarkdownLineParser markdownLineParser, IHtmlConverter htmlConverter, ITokensParser tokensParser)
    {
        this.markdownLineParser = markdownLineParser;
        this.htmlConverter = htmlConverter;
        this.tokensParser = tokensParser;
    }

    public string Render(string markdownLine)
    {
        var paragraphs = markdownLineParser.ParseMarkdownTextIntoParagraphs(markdownLine);
        
        var markdownParagraphs = (from paragraph in paragraphs
            select markdownLineParser.ParseParagraphForTokens(paragraph).ToList()
            into tokens
            let tags = tokensParser.ParserMarkdownTags(tokens).ToList()
            select new MarkdownParagraph { Tokens = tokens, Tags = tags }).ToList();

        var htmlText = htmlConverter.Convert(markdownParagraphs);
        
        return htmlText;
    }
}