using System;

namespace Markdown
{
	public class TextStream
	{
		private readonly string text;
		private int position;

		public TextStream(string text)
		{
			this.text = text ?? throw new ArgumentNullException(nameof(text));
		}

		public char Current() => text[position];

		public void MoveNext() => position++;
		public void MoveTo(int index) => position = index;

		public char Lookahead(int number)
		{
			var isIndexInBorders = position + number <= text.Length - 1 && position + number >= 0;
			return isIndexInBorders ? text[position + number] : '\0';
		}

		public string Text() => text;

		public int Length() => text.Length;

		public int Position() => position;
	}
}
