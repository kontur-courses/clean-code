using Markdown.Extensions;

namespace Markdown.Tokens;

public class LiteralToken : Token
{
    public override string TagWrapper { get; } = "";
    public override string Separator { get; } = "";
    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = false;
    public override bool IsContented { get; } = false;

    private string Content { get; set; }


    public LiteralToken(int openingIndex, int closingIndex, string content) : base(openingIndex, closingIndex)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentException();
        Content = content;
    }

    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        IsCorrect = !(this.IsSeparatorsInsideDifferentWords(str) || this.IsTokenHasNoContent());
    }

    public override string ToString()
    {
        return Content;
    }
}