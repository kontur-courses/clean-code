namespace Markdown.Content
{
    interface IContentFinder
    {
        (int lenght, string content) GetBlockquoteContent(string text, int index);
        (int lenght, string content) GetCodeContent(string text, int index);
        (int lenght, string content) GetEmContent(string text, int index);
        (int lenght, string content) GetHeaderContent(string text, int index);
        (int lenght, string content) GetListContent(string text, int index);
        (int lenght, string content) GetSimpleContent(string text, int index);
        (int lenght, string content) GetStrikeContent(string text, int index);
        (int lenght, string content) GetHorizontalContent(string text, int index);
        (int lenght, string content) GetStrongContent(string text, int index);
        (int lenght, string content) GetDefaultContent(string text, int index);
    }
}
