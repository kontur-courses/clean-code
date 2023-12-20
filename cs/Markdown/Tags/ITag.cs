namespace Markdown;

public interface ITag
{
    public ITag CreateToken(string content)
    {
        return this;
    }
}