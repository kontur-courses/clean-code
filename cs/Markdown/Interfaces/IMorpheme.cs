namespace Markdown.Interfaces
{
    public interface IMorpheme : ITag
    {
        bool CheckForCompliance(string textContext, int position);

        MorphemeType MorphemeType { get; }
    }
}