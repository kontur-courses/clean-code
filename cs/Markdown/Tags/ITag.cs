using System.Collections.Generic;

namespace Markdown
{
    public interface ITag
    {
        string OpeningMarkup { get; }
        string ClosingMarkup { get; }
        string OpeningTag { get; }
        string ClosingTag { get; }
        bool IsBrokenMarkup(string source, int start, int length);
        void Replace(List<string> builder, int start, int end);
    }
}