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
			var body = text.ToString(1, text.Length - 2);
			return $"{htmlOpenTag}{body}{htmlCloseTag}";
		}
	}
}