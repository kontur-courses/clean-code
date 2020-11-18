using System.Text;

namespace Markdown.TagConverters
{
    internal interface ITagConverter : IIsTag
    {
        public string TagName { get; }
        public string TagHtml { get; }
        public bool IsSingleTag { get; }
        public StringBuilder Convert(StringBuilder tagsText, StringBuilder text, int start, int finish);

        public bool CanProcessTag(string tagName);

        public bool CanOpen(StringBuilder text, int pos);

        public bool CanClose(StringBuilder text, int pos);
    }
}
