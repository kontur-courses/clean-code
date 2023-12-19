using System.Text;

namespace Markdown
{
    public class Tag
    {
        public Tag()
        {
        }

        public Tag(StringBuilder? markdownText, int index)
        {
            Text = markdownText;
            Index = index;
        }

        public StringBuilder? Text;
        public int Index;
        public List<Tag> NestedTags = new();
    }
}