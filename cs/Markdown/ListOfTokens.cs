namespace Markdown;

public class ListOfTokens<T> : List<T>
{
    public new void Add(T token)
    {
        if (!(token is null))
            base.Add(token);
    }
}