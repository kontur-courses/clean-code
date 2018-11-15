using System;
using System.Collections.Generic;
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
			{"__", new DoubleUnderLineTag()},
			{"#", new SharpTag()}
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
				var twoSymbol = text.Substring(position, 2);
				if (IsOpenTag(twoSymbol, text) && TryGetOnePairOfTags(twoSymbol, text, out var tag))
				{
					pairedTags.Add(tag);
					position = tag.CloseIndex + tag.Length - 1;
				}

				var symbol = text[position].ToString();
				if (IsOpenTag(symbol, text) && TryGetOnePairOfTags(symbol, text, out tag))
				{
					pairedTags.Add(tag);
					position = tag.CloseIndex + tag.Length - 1;
				}

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

		private bool TryGetOnePairOfTags(string symbol, string text, out ITag tag)
		{
			var tagType = dictionaryTags[symbol].GetType();
			tag = (ITag) Activator.CreateInstance(tagType);
			tag.OpenIndex = position;
			tag.CloseIndex = text.FindCloseTagIndex(tag);

			return tag.CloseIndex != -1;
		}
	}
}