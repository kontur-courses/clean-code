namespace Markdown.Tag
{
	public class SingleUnderLineTag : ITag
	{
		public string Symbol { get; set; } = "_";
		public int Length { get; set; } = 1;
		public int OpenIndex { get; set; }
		public int CloseIndex { get; set; }
		public string HtmlOpen { get; set; } = "<em>";
		public string HtmlClose { get; set; } = "</em>";
	}
}