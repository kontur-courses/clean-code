using System;

namespace Markdown
{
    public interface ITagInfo
    {
        Predicate<CollectionView<char>> StartCondition { get; }
        Predicate<CollectionView<char>> EndCondition { get; }
        Action<TagReader> OnTagStart { get; }
        Action<TagReader> OnTagEnd { get; }
        string HtmlTagText { get; }
        string MarkdownTagText { get; }
        int TagLength { get; }
        Token GetNewToken(int position);
    }
}
