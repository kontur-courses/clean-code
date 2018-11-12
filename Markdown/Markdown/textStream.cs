using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class TextStream
	{
		private readonly string text;
		private int position;

		public TextStream(string text)
		{
			this.text = text ?? throw new ArgumentNullException("The text should not be null");
		}

		public char Current() => text[position];

		public void MoveNext() => position++;

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
