using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class MdConverter
	{
		private readonly string text;
		private int position;
		private readonly List<ITag> tagStorage;
		private readonly StringBuilder textStorage;
		private readonly List<ITag> stopTags;
		private readonly List<string> result;

		public MdConverter(string text)
		{
			this.text = text;
			textStorage = new StringBuilder();
			result = new List<string>();
			stopTags = new List<ITag> { new UnderLineTag() };
			tagStorage = new List<ITag>();
		}

		public string ConvertToHtml()
		{
			while (position < text.Length)
			{
				var symbol = text[position];
				var isEndOfText = position == text.Length - 1;
				var prevSymbol = position == 0 ? '^' : text[position - 1];
				var nextSymbol = isEndOfText ? '^' : text[position + 1];

				if (isEndOfText)
				{
					AddLast(symbol);
					break;
				}

				if (stopTags.All(s => s.Symbol != symbol))
					textStorage.Append(symbol);

				if (IsCloseTag(symbol, nextSymbol, prevSymbol))
				{
					var tag = stopTags.Single(s => s.Symbol == symbol);
					var wrappedText = tag.Wrap(textStorage);
					result.Add(wrappedText);
					Clear(symbol);
				}

				if (IsOpenTag(symbol, nextSymbol, prevSymbol))
				{
					result.Add(textStorage.ToString());
					var tag = stopTags.Single(s => s.Symbol == symbol);
					tagStorage.Add(tag);
					textStorage.Clear();
				}

				position++;
			}

			return string.Join("", result);
		}

		private bool IsOpenTag(char symbol, char nextSymbol, char prevSymbol) =>
			stopTags.Any(s => s.Symbol == symbol) && !char.IsWhiteSpace(nextSymbol)
			                                      && (char.IsWhiteSpace(prevSymbol) || position == 0);

		private bool IsCloseTag(char symbol, char nextSymbol, char prevSymbol) =>
			tagStorage.Any(t => t.Symbol == symbol) && char.IsWhiteSpace(nextSymbol)
													&& !char.IsWhiteSpace(prevSymbol);



		private void Clear(char symbol)
		{
			textStorage.Clear();
			var tag = tagStorage.Single(t => t.Symbol == symbol);
			tagStorage.Remove(tag);
		}

		private void AddLast(char symbol)
		{
			if (tagStorage.Any(t => t.Symbol == symbol))
			{
				var tag = stopTags.Single(s => s.Symbol == symbol);
				var wrappedText = tag.Wrap(textStorage);
				result.Add(wrappedText);
				Clear(symbol);
				return;
			}

			if (tagStorage.All(t => t.Symbol != symbol))
			{
				textStorage.Append(symbol);
				result.Add(textStorage.ToString());
				textStorage.Clear();
			}
		}
	}
}
