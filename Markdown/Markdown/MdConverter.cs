using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverter
	{
		private int position;

		public readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
		{
			{"_", new SingleUnderLineTag()},
			{"__", new DoubleUnderLineTag()}
		};

		public string ConvertToHtml(string text)
		{
			var existingPairedTags = GetAllPairedTags(text);

			if (existingPairedTags.Count == 0)
				return text;

			var replacementTags = new ReplacementTags(existingPairedTags, text);
			var textInHtml = replacementTags.ReplaceToHtml();

			return textInHtml;
		}

		public List<ITag> GetAllPairedTags(string text)
		{
			var pairedTags = new List<ITag>();
			position = 0;

			while (position < text.Length - 2)
			{
				var symbol = text[position].ToString();
				var twoSymbol = text.Substring(position, 2);

				if (twoSymbol.IsOpenTag(text, position, dictionaryTags, 2))
					pairedTags.AddRange(GetOnePairOfTags(twoSymbol, text));
				if (symbol.IsOpenTag(text, position, dictionaryTags, 1))
					pairedTags.AddRange(GetOnePairOfTags(symbol, text));
				else
					position++;
			}

			return pairedTags;
		}

		private List<ITag> GetOnePairOfTags(string symbol, string text)
		{
			var pairedTags = new List<ITag>();
			var tagType = dictionaryTags[symbol].GetType();
			if (Activator.CreateInstance(tagType) is ITag tag)
			{
				tag.OpenIndex = position;
				tag.CloseIndex = text.FindCloseTagIndex(tag);

				if (tag.CloseIndex != -1)
				{
					pairedTags.Add(tag);
					position = pairedTags.Last().CloseIndex + pairedTags.Last().Length;
				}
				else
				{
					position++;
				}
			}

			return pairedTags;
		}
	}
}