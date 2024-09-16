using Markdown.Lexer;
using Markdown.TokenConverter;

namespace Markdown.Renderer;

public class MarkdownRenderer : IMarkdownRenderer
{
    private readonly ILexer lexer;
    private readonly ITokenConverter tokenConverter;

    public MarkdownRenderer(ILexer lexer, ITokenConverter tokenConverter)
    {
        this.lexer = lexer;
        this.tokenConverter = tokenConverter;
    }

    public string Render(string text)
    {
        if (text is null)
            throw new ArgumentException("Input parameter cannot be null.");

        var lines = text.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

        var renderedLines = lines
            .Select(RenderLine)
            .ToList();

        return string.Join('\n', AddOuterTags(renderedLines));
    }

    private static string[] AddOuterTags(List<TokenConversionResult> lines)
    {
        var withOuterTags = new string[lines.Count];
        var indexes = OuterTagsProcessor.GetPositionsWithOuterTags(lines);
        
        var isOpeningTag = true;
        foreach (var index in indexes)
        {
            if (isOpeningTag)
            {
                withOuterTags[index] = lines[index].OuterTag!.OpeningTag + lines[index].ConvertedTokens;
                isOpeningTag = false;
                continue;
            }
            
            if (withOuterTags[index] is not null)
                withOuterTags[index] += lines[index].OuterTag!.ClosingTag;
            else
                withOuterTags[index] = lines[index].ConvertedTokens + lines[index].OuterTag!.ClosingTag;

            isOpeningTag = true;
        }
        
        for (var i = 0; i < lines.Count; i++)
        {
            if (withOuterTags[i] is not null)
                continue;
            withOuterTags[i] = lines[i].ConvertedTokens;
        }

        return withOuterTags;
    }

    private TokenConversionResult RenderLine(string line)
        => tokenConverter.ConvertToString(lexer.Tokenize(line));
}