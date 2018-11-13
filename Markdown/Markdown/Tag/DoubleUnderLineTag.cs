namespace Markdown.Tag
{
	class DoubleUnderLineTag : ITag
	{
		public string Symbol { get; set; } = "__";
		public int Length { get; set; } = 2;
		public int OpenIndex { get; set; }
		public int CloseIndex { get; set; }
		public string HtmlOpen { get; set; } = "<strong>";
		public string HtmlClose { get; set; } = "</strong>";

		public int FindCloseIndex(TextStream stream)
		{
			var text = stream.Text;
			for (var i = OpenIndex + 2; i < text.Length - 1; i++)

			{
				var symbolAfterTag = stream.LookAt(i + Length);
				var symbolBeforeTag = stream.LookAt(i - 1);

				if (text.Substring(i, Length) == Symbol && (char.IsWhiteSpace(symbolAfterTag) || i == text.Length - Length)
				                                        && char.IsLetter(symbolBeforeTag))
					return i;
			}

			return -1;
		}

		public string Body(string text) => text.Substring(OpenIndex + Length, CloseIndex - OpenIndex - Length);
	}
}
