﻿using System;

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
		public int FindCloseIndex(string text)
		{
			var stream = new TextStream(text);
			for (var i = OpenIndex + 2; i < text.Length; i++)
			{
				var nextSymbol = stream.Lookahead(i + 2);
				var prevSymbol = stream.Lookahead(i - 1);
				if (text.Substring(i, 2) == Symbol && (char.IsWhiteSpace(nextSymbol) || i == text.Length - 2)
					                                && !char.IsWhiteSpace(prevSymbol))
					return i;
			}

			return -1;
		}
	}
}
