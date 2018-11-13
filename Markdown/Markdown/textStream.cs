using System;

namespace Markdown
{
	public class TextStream
	{
		public TextStream(string text)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
		}

		public string Text { get; private set; }
		public int Position { get; private set; }

		public char Current() => Text[Position];

		public void MoveNext() => Position++;

		public void MoveTo(int index) => Position = index;

		public char Lookahead(int number)
		{
			var isIndexInBorders = Position + number <= Text.Length - 1 && Position + number >= 0;
			return isIndexInBorders ? Text[Position + number] : '\0';
		}

		public char LookAt(int index)
		{
			var isIndexInBorders = index <= Text.Length - 1 && index >= 0;
			return isIndexInBorders ? Text[index] : '\0';
		}
	}
}
