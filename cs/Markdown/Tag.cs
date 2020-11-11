namespace Markdown
{
    public class Tag
    {
        public readonly string TagName;
        public readonly int Position;
        public readonly bool IsPairTag;
        public int TagLenght => TagName.Length;

        public Tag(string tagName, int position, bool isPairTag = true)
        {
            TagName = tagName;
            Position = position;
            IsPairTag = isPairTag;
        }


        public string BuildHtmlTag(bool isOpenTeg)
        {
            var closMark = isOpenTeg ? "" : "/";
            return TagName != "\\" ? $"<{closMark}{TagConvertor.ConvertMdToHtml(TagName)}>" : "";
        }
    }
}