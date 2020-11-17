using System.Text;

namespace Markdown.TagConverters
{
    internal interface ITagConverter
    {
        public StringBuilder Convert(StringBuilder tagsText, StringBuilder text, int start, int finish);
    }
}
