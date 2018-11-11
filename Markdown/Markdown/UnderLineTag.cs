using System.Text;

namespace Markdown
{
	public class UnderLineTag : ITag
	{
		public char Symbol { get; set; } = '_';
		private readonly string htmlOpenTag = "<em>";
		private readonly string htmlCloseTag = "</em>";

		public string Wrap(StringBuilder text)
		{
			return $"{htmlOpenTag}{text}{htmlCloseTag}";
		}
	}
}