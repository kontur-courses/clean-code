namespace Markdown
{
	public class UnderLineTag : ITag
	{
		public char Symbol { get; set; } = '_';
		public int OpenIndex { get; set; }
		public int CloseIndex { get; set; }
		public string HtmlOpen { get; set; } = "<em>";
		public string HtmlClose { get; set; } = "</em>";

		public int FindCloseIndex(string text)
		{
			for (var i = OpenIndex + 2; i < text.Length; i++)
			{
				var nextSymbol = i == text.Length - 1 ? '^' : text[i + 1];
				if (text[i] == Symbol && (char.IsWhiteSpace(nextSymbol) || i == text.Length - 1)
				                      && !char.IsWhiteSpace(text[i - 1]))
					return i;
			}

			return -1;
		}
	}
}