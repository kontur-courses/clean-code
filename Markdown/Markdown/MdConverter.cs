using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverter
	{
		private int position;

		public static readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
		{
			{"_", new SingleUnderLineTag()},
			{"__", new DoubleUnderLineTag()}
		};

		public string ConvertToHtml(string text)
		{
			var existingPairedTags = GetAllPairedTags(text);

			if (existingPairedTags.Count == 0)
				return text.RemoveScreenCharacters();

			var textInHtml = GetHtmlCode(existingPairedTags, text);


			return textInHtml.RemoveScreenCharacters();
		}

		private List<ITag> GetAllPairedTags(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			var pairedTags = new List<ITag>();
			position = 0;

			while (position < text.Length - 2)
			{
				var symbol = text[position].ToString();
				var twoSymbol = text.Substring(position, 2);

				if (twoSymbol.IsOpenTag(text, position, 2))
					pairedTags.AddRange(GetOnePairOfTags(twoSymbol, text));

				if (symbol.IsOpenTag(text, position, 1))
					pairedTags.AddRange(GetOnePairOfTags(symbol, text));

				else
					position++;
			}

			return pairedTags;
		}

		private List<ITag> GetOnePairOfTags(string symbol, string text)
		{
			var pairedTags = new List<ITag>();
			var tag = dictionaryTags[symbol];
			tag.OpenIndex = position;
			tag.CloseIndex = text.FindCloseTagIndex(tag);

			if (tag.CloseIndex != -1)
			{
				pairedTags.Add(tag);
				position = pairedTags.Last().CloseIndex + pairedTags.Last().Length;
			}
			else
				position++;

			return pairedTags;
		}

		private string GetHtmlCode(List<ITag> pairedTags, string text)
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();

			foreach (var tag in pairedTags)
			{
				htmlBuilder.Append(text.Substring(startIndex, tag.OpenIndex - startIndex));
				htmlBuilder.Append(tag.HtmlOpen);
				htmlBuilder.Append(GetInnerFormattedText(tag, text));
				htmlBuilder.Append(tag.HtmlClose);
				htmlBuilder.Append(text.Substring(tag.CloseIndex + tag.Length));

				startIndex = tag.CloseIndex + tag.Length;
			}

			return htmlBuilder.ToString();
		}

		private string GetInnerFormattedText(ITag tag, string text)
		{
			var innerText = text.GetBodyInside(tag);
			var innerPairedTags = GetAllPairedTags(innerText);

			return innerPairedTags.Count != 0 && innerPairedTags.Last().Length > tag.Length || innerPairedTags.Count == 0
				? innerText
				: GetHtmlCode(innerPairedTags, innerText);
		}
	}
}