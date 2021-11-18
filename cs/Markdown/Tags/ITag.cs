using System;

namespace Markdown
{
    public interface ITag
    {
        string Opening { get; }
        string Closing { get; }
        int Priority { get; }
    }
}