namespace Markdown.Tag
{
	public interface ITag
	{
		string Symbol { get; set; }
		int OpenIndex { get; set; }
		int CloseIndex { get; set; }
		string HtmlOpen { get; set; }
		string HtmlClose { get; set; }
		int Length { get; set; }
	}
}