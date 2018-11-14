using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown
{
	public static class StringExtensions
	{
		public static string RemoveScreenCharacters(this string text) => text.Replace("\\", "");

		public static bool IsOpenTag(this string symbol, string text, int position, Dictionary<string, ITag> dictionaryTags,
			int tagLength)
		{
			var prevSymbol = text.LookAt(position - 1);
			var nextSymbol = text.LookAt(position + tagLength);

			return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
			                                          && (char.IsWhiteSpace(prevSymbol) || position == 0);
		}

		public static int FindCloseTagIndex(this string text, ITag tag)
		{
			for (var i = tag.OpenIndex + 2; i < text.Length - tag.Length + 1; i++)
			{
				var symbolAfterTag = text.LookAt(i + tag.Length);
				var symbolBeforeTag = text.LookAt(i - 1);

				if (text.Substring(i, tag.Length) == tag.Symbol && (char.IsWhiteSpace(symbolAfterTag) ||
				                                                    i == text.Length - tag.Length)
				                                                && char.IsLetter(symbolBeforeTag))
					return i;
			}

			return -1;
		}

		public static string GetBodyInside(this string text, ITag tag) =>
			text.Substring(tag.OpenIndex + tag.Length, tag.CloseIndex - tag.OpenIndex - tag.Length);

		public static char LookAt(this string text, int index)
		{
			var isIndexInBorders = index <= text.Length - 1 && index >= 0;
			return isIndexInBorders ? text[index] : '\0';
		}
	}
}
