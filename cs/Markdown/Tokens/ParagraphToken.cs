﻿using Markdown.Extensions;

namespace Markdown.Tokens;

public class ParagraphToken : Token
{
    public override string TagWrapper { get; } = "h1";
    public override string Separator { get; } = "# ";
    public override bool IsCanContainAnotherTags { get; } = true;
    public override bool IsSingleSeparator { get; } = true;
    public override bool IsContented { get; } = false;


    public ParagraphToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public ParagraphToken(int openingIndex) : base(openingIndex)
    {
    }

    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        IsCorrect = (OpeningIndex == 0 || str[OpeningIndex - 1] == '\n')
                    && (ClosingIndex == str.Length - 1
                        || str[ClosingIndex + 1] == '\n'
                        || str[ClosingIndex + 1] == '\r'
                    );
    }

    public override bool CanCloseToken(int closeIndex, string str)
    {
        if (str[closeIndex] == '\n' || closeIndex == str.Length - 1)
            return true;
        return false;
    }
}