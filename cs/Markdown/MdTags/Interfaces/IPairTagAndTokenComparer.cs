namespace Markdown.MdTags.Interfaces
{
    interface IPairTagAndTokenComparer
    {
        bool IsTokenOpenTag(Token token, Tag openTag);
        bool IsTokenCloseTag(Token token, Tag closeTag);
    }
}