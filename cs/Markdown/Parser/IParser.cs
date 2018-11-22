namespace Markdown
{
    public interface IParser
    {
        Tag Parse(string str);
    }
}