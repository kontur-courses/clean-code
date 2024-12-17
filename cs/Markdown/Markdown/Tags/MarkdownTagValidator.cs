using Markdown.Markdown.Tags.Image;
using Markdown.Markdown.Tags.Underscore;

namespace Markdown.Markdown.Tags;

public static class MarkdownTagValidator
{
    private static readonly ITagValidator[] TagValidators = 
    [
        new StrongTagValidator(),
        new ItalicTagValidator(),
        new HeaderTagValidator(),
        new StartImageTagValidator(),
        new MiddleImageTagValidator(),
        new EndImageTagValidator()
    ];

    public static bool Validate(string content) =>
        TagValidators.Any(validator => validator.IsValidTag(content));

    public static bool IsTagStart(string content) =>
        TagValidators.Any(validator => validator.IsTagStart(content));
    
    public static bool IsTagEnd(string content) =>
        TagValidators.Any(validator => validator.IsTagEnd(content));
}