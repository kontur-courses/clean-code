﻿namespace Markdown.Tag;

public interface ITag
{
    string OpeningSeparator { get; }
    string CloseSeparator { get; }
    bool IsPaired { get; }
    void RenderParameters(List<string> values, IList<string> parameters);
}