using Markdown.Html.Tags; 
using Markdown.Markdown.Tokens; 
 
namespace Markdown.Markdown.Handlers; 
 
public class TagTypeHandler : ITokenHandler 
{ 
    private static readonly Dictionary<string, TagType> StringToTag = new() 
    { 
        { MarkdownConstants.SingleUnderscore, TagType.Italic }, 
        { MarkdownConstants.DoubleUnderscore, TagType.Strong }, 
        { MarkdownConstants.Header, TagType.Header },
        { MarkdownConstants.StartImage, TagType.Image },
        { MarkdownConstants.MiddleImage, TagType.Image },
        { MarkdownConstants.EndImage, TagType.Image }
    }; 
 
    public IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens) =>
         tokens.Select(t => StringToTag.TryGetValue(t.Content, out var tagType)
                ? MarkdownTokenCreator.CreateOpenTag(t, tagType)
                : t)
            .ToList();
}