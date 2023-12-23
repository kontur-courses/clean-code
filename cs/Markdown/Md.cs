namespace Markdown;

public class Md
{
    public string Render(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException();
        return "";
    }
}