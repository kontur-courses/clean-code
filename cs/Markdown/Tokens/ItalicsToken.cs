﻿using Markdown.Extensions;

namespace Markdown.Tokens;

public class ItalicsToken:Token
{
    public override string TagWrapper { get; } = "em";
    public override string Separator { get; } = "_";
    public override bool IsCanContainAnotherTags { get; } = true;
    public override bool IsSingleSeparator { get; } = false;
    
    public ItalicsToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex) {}
    public ItalicsToken(int openingIndex) : base(openingIndex){}

    public override void Validate(string str)
    {
        IsCorrect = !this.IsSeparatorsInsideDifferentWords(str);
    }
}