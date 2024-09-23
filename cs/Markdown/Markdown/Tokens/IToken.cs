﻿using Markdown.Tags.TextTag;

namespace Markdown.Tokens;

public interface IToken<T> where T : IConvertableToString
{
    T Value { get; }
    int StartIndex { get; }
    int EndIndex { get; }
}