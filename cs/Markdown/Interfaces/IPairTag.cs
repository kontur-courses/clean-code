namespace Markdown.Interfaces
{
    public interface IPairTag : ITag
    {
        bool CheckForCompliance(string textContext, int position);
    }
}