using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public class MdConverter
	{
		private readonly string text;
		private int position;
		private readonly List<ITag> existedTags;
		readonly Dictionary<char, ITag> dictionaryTags;

		public MdConverter(string text)
		{
			this.text = text ?? throw new ArgumentNullException("The text should not be null");
			existedTags = new List<ITag>();
			dictionaryTags = new Dictionary<char, ITag>
			{
				{'_', new UnderLineTag()}
			};
		}

		public string ConvertToHtml()
		{
			while (position < text.Length)
			{
				var symbol = text[position];

				if (IsOpenTag(symbol))
				{
					var tag = dictionaryTags[symbol];
					tag.OpenIndex = position;
					existedTags.Add(tag);
				}

				position++;
			}

			if (existedTags.Count == 0)
				return text.Replace("\\", "");

			foreach (var tag in existedTags)
			{
				var closeIndex = tag.FindCloseIndex(text);
				if (closeIndex == -1)
					existedTags.Remove(tag);
				tag.CloseIndex = closeIndex;
			}

			var sortedTagIndexes = GetSortedTagIndexes();
			var textInHtml = TransformToHtml(sortedTagIndexes);


			return textInHtml.Replace("\\", "");
		}

		private string TransformToHtml(List<int> sortedTagIndexes)
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();
			foreach (var tagIndex in sortedTagIndexes)
			{
				htmlBuilder.Append(text.Substring(startIndex, tagIndex - startIndex));
				var isOpen = existedTags.Any(t => t.OpenIndex == tagIndex);

				if (isOpen)
				{
					var tag = existedTags.Single(t => t.OpenIndex == tagIndex);
					htmlBuilder.Append(tag.HtmlOpen);
				}
				else
				{
					var tag = existedTags.Single(t => t.CloseIndex == tagIndex);
					htmlBuilder.Append(tag.HtmlClose);
				}

				startIndex = tagIndex + 1;
			}

			return htmlBuilder.ToString();
		}

		private List<int> GetSortedTagIndexes()
		{
			var tagIndexes = existedTags.Select(t => t.OpenIndex).ToList();
			tagIndexes.AddRange(existedTags.Select(t => t.CloseIndex));
			tagIndexes.Sort();
			return tagIndexes;
		}

		private bool IsOpenTag(char symbol)
		{
			var prevSymbol = position == 0 ? '-' : text[position - 1];
			var nextSymbol = position == text.Length - 1 ? '-' : text[position + 1];

			return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
			                                          && (char.IsWhiteSpace(prevSymbol) || position == 0);
		}
	}
}
