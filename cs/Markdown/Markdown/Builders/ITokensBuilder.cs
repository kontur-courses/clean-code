﻿using Markdown.Tags.TextTag;
using Markdown.Tokens;

namespace Markdown.Builders;

public interface ITokensBuilder<T> where T : IConvertableToString
{
    string Build(List<IToken<T>> tokens);
}