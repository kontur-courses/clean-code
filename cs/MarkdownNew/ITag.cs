namespace Markdown
{
    interface ITag
    {
        bool IsValidOpenTagFromPosition(string convertingString, int position);
        bool IsValidCloseTagFromPosition(string convertingString, int position);
    }
}
