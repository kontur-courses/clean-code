using Markdown.HtmlConverting;
using Markdown.MdParsing.Interfaces;
using Markdown.TokenParsers.TokenHandlers;
using Markdown.TokenParsers.TokenReaders;
using Markdown.Tokens;

namespace Markdown.MdParsing;

public class Md
{
    private readonly ITokensConverter converter = new TokensToHtmlConverter();
    private readonly ITokenReader reader = new CommonTokensReader();
    private readonly List<ITokenHandler> tokenHandlers = CreateHandlers();

    private static List<ITokenHandler> CreateHandlers()
    {
        return new List<ITokenHandler>
            { 
                new EscapeTokenApplyHandler(), 
                new IncorrectTagTokensHandler(),
                new UnderscoreInNumberHandler(),
                new TagsPartsInTagHandler(TagType.LinkValue),
                new StrongTagsHandler(),
                new TagTokensIntersectsHandler(TagType.Italic, TagType.Strong),
                new NonPairTagTokensHandler(),
                new NestedTagTokensHandler(TagType.Italic, TagType.Strong)
            }
            .OrderBy(x => x.Priority)
            .ToList();
    }

    public string Render(string text)
    {
        IReadOnlyList<Token> tokens = reader.ReadTokens(text).ToList();
        
        foreach (var tokenHandler in tokenHandlers)
            tokens = tokenHandler.Handle(tokens);
        
        return converter.Convert(tokens);
    }
}