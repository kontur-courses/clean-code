﻿using Markdown.Token;

namespace Markdown.Tags.HtmlTags;

public record HtmlHeadingTag() : Tag("<h1>", "</h1>")
{
    public override bool ClosesOnNewLine => false;
    public override bool CanIntersect => true;

    public override bool IsValidOpening(Stack<TokenTree> stack, string text, int position)
        => IsOpeningSequence(text, position);

    public override bool IsValidClosing(Stack<TokenTree> stack, string text, int position) =>
        IsClosingSequence(text, position);
}