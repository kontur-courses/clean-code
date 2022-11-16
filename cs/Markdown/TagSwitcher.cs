using System.Text;

namespace Markdown
{
    public class TagSwitcher
    {
        public string Switch(List<TagInfo> tags, string paragraph)
        {
            StringBuilder result = new(paragraph);
            foreach (var tag in tags)
            {
                SwitchTag(paragraph, tag);
                AdjustPositions(tags, tag);
            }
            RemoveEscapes(result);
            return result.ToString();
        }

        public void AdjustPositions(List<TagInfo> tags, TagInfo tag)
        {

        }

        public string SwitchTag(string paragraph, TagInfo tag)
        {
            return "";
        }

        public void RemoveEscapes(StringBuilder paragraph)
        {

        }
    }
}
