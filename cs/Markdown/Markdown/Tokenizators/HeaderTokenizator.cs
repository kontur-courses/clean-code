namespace Markdown;

public class HeaderTokenizator : ITokenizator
{
    private List<TagToken> result;

    public HeaderTokenizator() : base()
    {
        result = new();
    }

    public Tag Tag => new HeaderTag();
    public string OpenTag => Tag.OpenMdTag;
    public string CloseTag => Tag.CloseMdTag;

    public List<TagToken> Tokenize(string mdstring)
    {
        int i = 0, j;
        while (true)
        {
            i = mdstring.IndexOf(OpenTag, i);
            if (i == -1)
                break;
            j = mdstring.IndexOf(CloseTag, i + 1);
            if (j == -1)
            {
                result.Add(new TagToken(i, mdstring.Length - 1, Tag));
                break;
            }

            result.Add(new TagToken(i, j, Tag));
            i = j;
        }

        return result;
    }
}