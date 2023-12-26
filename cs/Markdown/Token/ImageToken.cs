namespace Markdown.Token;

public class ImageToken : IToken
{
    private const string TokenSeparator = "@";
    private const bool HasPair = true;

    private const char ImageStartSymbol = '!';
    private const char SourceEndingSymbol = ']';
    private const char DescriptionStartingSymbol = '(';
    private const char DescriptionEndingSymbol = ')';

    public int Position { get; }
    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public bool IsParametrized => true;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    private bool hasOpenedSource = false;
    private bool hasClosedSource = false;
    private bool hasOpenedDescription = false;
    private int descriptionPostition;

    public ImageToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        return ValidateTag(source, tokens, (ImageToken)currentToken);
    }

    private bool ValidateTag(string source, List<IToken> tokens, ImageToken currentToken)
    {
        if (!currentToken.hasOpenedSource && source[Position] == ImageStartSymbol)
        {
            currentToken.hasOpenedSource = true;
            Parameters = new List<string>();
            return true;
        }

        if (!currentToken.hasClosedSource && source[Position] == SourceEndingSymbol)
        {
            currentToken.hasClosedSource = true;
            currentToken.Parameters.Add(source.Substring(currentToken.Position + 2,
                Position - currentToken.Position - 2));
        }
        else if (currentToken.hasClosedSource && source[Position] == DescriptionStartingSymbol)
        {
            currentToken.hasOpenedDescription = true;
            currentToken.descriptionPostition = Position;
        }
        else if (currentToken.hasOpenedDescription && source[Position] == DescriptionEndingSymbol)
        {
            currentToken.Parameters.Add(source.Substring(currentToken.descriptionPostition + 1,
                Position - currentToken.descriptionPostition - 1));
            currentToken.TokenSymbolsShift = currentToken.Parameters[0].Length + currentToken.Parameters[1].Length + 4;
            return true;
        }

        return false;
    }
}