using System.Text;

namespace Markdown;

public class Tags
{
    private List<Tag> tagList;

    public Tags()
    {
        tagList = new List<Tag>();
    }

    public void AddTag(Tag tag)
    {
        tagList.Add(tag);
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        foreach (var tag in tagList)
        {
            result.AppendLine(tag.ToString());
        }

        return result.ToString();
    }
}