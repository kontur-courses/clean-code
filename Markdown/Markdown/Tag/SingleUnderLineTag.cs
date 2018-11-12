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

		public int FindCloseIndex(string text)
		{
			for (var i = OpenIndex + 2; i < text.Length; i++)
			{
				var nextSymbol = i == text.Length - 1 ? '^' : text[i + 1];
				if (text[i].ToString() == Symbol && (char.IsWhiteSpace(nextSymbol) || i == text.Length - 1)
				                      && !char.IsWhiteSpace(text[i - 1]))
					return i;
			}

			return -1;
		}

		public string Body(string text) => text.Substring(OpenIndex + Length, CloseIndex - OpenIndex - Length);
	}
}