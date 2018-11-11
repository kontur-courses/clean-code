using System.Text;

namespace Markdown
{
	interface ITag
	{
		char Symbol { get; set; }
		int OpenIndex { get; set; }
		int CloseIndex { get; set; }
		string HtmlOpen { get; set; }
		string HtmlClose { get; set; }
		int FindCloseIndex(string text);
	}
}