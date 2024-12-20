namespace Markdown.MdTags.Interfaces;

internal interface IHtmlConverter
{
    public string GetHtmlString(string mdString, int startIndex);
}
