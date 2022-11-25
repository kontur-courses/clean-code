using MarkdownProcessor.Tags;

namespace MarkdownProcessor.Tags;

public class FirstHeader : ITag
{
    public FirstHeader(Token openingToken)
    {
        OpeningToken = openingToken;
    }

    public ITagMarkdownConfig Config { get; } = new FirstHeaderConfig();
    public Token OpeningToken { get; }
    public Token ClosingToken { get; set; }
    public List<ITag> Children { get; } = new();
    public bool Closed { get; private set; }

    public Token? RunTokenDownOfTree(Token token)
    {
        if (Closed) throw new InvalidOperationException();

        if (Children.Any() && !Children.Last().Closed)
        {
            var nullableToken = Children.Last().RunTokenDownOfTree(token);
            if (nullableToken is null) return nullableToken;
            token = nullableToken.Value;
        }

        if (token.Value == Config.ClosingSign)
        {
            ClosingToken = token;
            Closed = true;
            return null;
        }

        return token;
    }

    public void RunTagDownOfTree(ITag tag)
    {
        if (tag is not (Bold or Italic)) return;

        if (Closed) throw new InvalidOperationException();

        if (Children.Any() && !Children.Last().Closed)
            Children.Last().RunTagDownOfTree(tag);
        else
            Children.Add(tag);
    }
}