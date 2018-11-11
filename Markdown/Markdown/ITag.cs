using System.Text;

namespace Markdown
{
	interface ITag
	{
		char Symbol { get; set; }
		string Wrap(StringBuilder text);
	}
}