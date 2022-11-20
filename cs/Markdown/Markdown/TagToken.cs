namespace Markdown;

public class TagToken
{
    public int leftBorder, rightBorder;
    public Tag tag;

    public TagToken(int leftBorder, int rightBorder, Tag tag)
    {
        this.leftBorder = leftBorder;
        this.rightBorder = rightBorder;
        this.tag = tag;
    }
}