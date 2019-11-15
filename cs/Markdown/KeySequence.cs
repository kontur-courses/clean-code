using System;

namespace Markdown
{
	public struct KeySequence
	{
		public string Sequence { get; }

		public char this[int index] => Sequence[index];

		public KeySequence(string keySequence)
		{
			if (string.IsNullOrEmpty(keySequence))
				throw new ArgumentException("Key sequence can't be null or empty");
			Sequence = keySequence;
		}

		public override int GetHashCode() => Sequence.GetHashCode();

		public override bool Equals(object obj) => Sequence.Equals(obj);

		public static implicit operator KeySequence(string keySequence) => new KeySequence(keySequence);
		
		public static implicit operator KeySequence(char keyChar) => new KeySequence(keyChar.ToString());
	}
}