using System;

namespace Markdown
{
    public interface ITag
    {
        string OpeningMarkup { get; }
        string ClosingMarkup { get; }
        string OpeningTag { get; }
        string ClosingTag { get; }
    }
}