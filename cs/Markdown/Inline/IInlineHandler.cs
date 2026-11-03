namespace Markdown.Inline
{
    public interface IInlineHandler
    {
        bool TryConvertToHtml(string text, ref int position, out string result);
    }
}