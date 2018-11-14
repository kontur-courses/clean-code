using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverter
	{
		private int position;
		private readonly List<string> escapedSymbols = new List<string> {"\\"};
		public readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
		{
			{"_", new SingleUnderLineTag()},
			{"__", new DoubleUnderLineTag()}
		};

		public string ConvertToHtml(string text)
		{
			var existingPairedTags = GetAllPairedTags(text);

			text = text.RemoveEscapedSymbols(escapedSymbols, dictionaryTags);

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

			while (position < text.Length - 1)
			{
				var symbol = text[position].ToString();
				var twoSymbol = text.Substring(position, 2);

				if (IsOpenTag(twoSymbol, text))
					pairedTags.AddRange(GetOnePairOfTags(twoSymbol, text));

				if (IsOpenTag(symbol, text))
					pairedTags.AddRange(GetOnePairOfTags(symbol, text));

				position++;
			}

			return pairedTags;
		}

		public bool IsOpenTag(string symbol, string text)
		{
			var prevSymbol = text.LookAt(position - 1);
			var nextSymbol = text.LookAt(position + symbol.Length);

			return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
			                                          && (char.IsWhiteSpace(prevSymbol) || position == 0);
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
					position = pairedTags.Last().CloseIndex + pairedTags.Last().Length - 1;
				}
			}

			return pairedTags;
		}
	}
}