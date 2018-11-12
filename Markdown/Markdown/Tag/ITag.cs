namespace Markdown.Tag
{
	interface ITag
	{
		string Symbol { get; set; }
		int OpenIndex { get; set; }
		int CloseIndex { get; set; }
		string HtmlOpen { get; set; }
		string HtmlClose { get; set; }
		int Length { get; set; }
		int FindCloseIndex(string text);
		string Body(string text);
	}
}