using System;

namespace Markdown
{
    public interface ITagInfo
    {
        Predicate<StringView> StartCondition { get; }
        Predicate<StringView> EndCondition { get; }
        Action<TagReader> OnTagStart { get; }
        Action<TagReader> OnTagEnd { get; }
        string HtmlTagText { get; }
        string MarkdownTagText { get; }
        int TagLength { get; }
        Token GetNewToken(int position);
    }
}
