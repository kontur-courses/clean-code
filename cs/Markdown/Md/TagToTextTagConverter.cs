namespace Markdown.Md
{
    public class TagToTextTagConverter : ITagConverter
    {
        public Tag Convert(Tag tag)
        {
            return new Tag {Type = MdSpecification.Text, Value = tag.Value, Tags = tag.Tags};
        }
    }
}