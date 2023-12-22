namespace Markdown.Token;

public class ImageToken : IToken
{
    private const string TokenSeparator = "@";
    private const bool HasPair = true;

    public int Position { get; }
    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public bool IsParametrized => true;
    public List<string> Parameters { get; set; }
    public int Shift { get; set; }
    
    public bool HasOpenedSource = false;
    public bool HasClosedSource = false;
    public bool HasOpenedDescription = false;
    public int DescriptionPostition;

    public ImageToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, ref List<IToken> tokens, IToken currentToken)
    {
        return ValidateTag(source, ref tokens, (ImageToken)currentToken);
    }

    private bool ValidateTag(string source, ref List<IToken> tokens, ImageToken currentToken)
    {
        if (!currentToken.HasOpenedSource && source[Position] == '!')
        {
            currentToken.HasOpenedSource = true;
            Parameters = new List<string>();
            return true;
        }

        if (!currentToken.HasClosedSource && source[Position] == ']')
        {
            currentToken.HasClosedSource = true;
            currentToken.Parameters.Add(source.Substring(currentToken.Position + 2, Position - currentToken.Position - 2));
        }
        else if (currentToken.HasClosedSource && source[Position] == '(')
        {
            currentToken.HasOpenedDescription = true;
            currentToken.DescriptionPostition = Position;
        }
        else if (currentToken.HasOpenedSource && source[Position] == ')')
        {
            currentToken.Parameters.Add(source.Substring(currentToken.DescriptionPostition + 1,
                Position - currentToken.DescriptionPostition - 1));
            currentToken.Shift = currentToken.Parameters[0].Length + currentToken.Parameters[1].Length + 4;
            return true;
        }

        return false;
    }
}