namespace Markdown.Rules
{
    public interface IRules
    {
        bool IsSeparatorValid(string text, int position, bool isFirst);
    }
}