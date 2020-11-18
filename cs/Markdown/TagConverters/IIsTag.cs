using System.Text;

namespace Markdown.TagConverters
{
    interface IIsTag
    {
        public bool IsTag(string text, int pos);
    }
}
