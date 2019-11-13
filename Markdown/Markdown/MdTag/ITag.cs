namespace Markdown.MdTag
{
    public interface ITag
    {
        string WrapTagIntoHtml();

        void AddNestedTag(Tag tag);

        void AddTagContent(string content);
    }
}
