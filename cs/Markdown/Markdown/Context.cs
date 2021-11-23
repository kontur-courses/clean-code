using System;

namespace Markdown
{
    public class Context
    {
        public string Text { get; }
        public int Index { get; set; }

        public char CurrentSymbol
        {
            get
            {
                if (Index >= Text.Length)
                    throw new ArgumentOutOfRangeException(nameof(Index), "Index out of context.");

                return Text[Index];
            }
        }

        public Context(string text, int index = 0)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Index = index;
        }

        public bool StartsWith(string value) => Text[Index..].StartsWith(value);

        public bool IsStringBeginning() => Index == 0;

        public bool IsStringEnding() => Index == Text.Length - 1;

        public Context Skip(int count) => new(Text, Index + count);

        public bool CharAfter(string value, Func<char, bool> checker) =>
            Text.Length > Index + value.Length
            && checker(Skip(value.Length).CurrentSymbol);

        public bool CharAfter(Func<char, bool> checker) => Skip(1).CharAfter("", checker);
        
        public bool CharBefore(Func<char, bool> checker) =>
            Index - 1 >= 0
            && checker(Skip(-1).CurrentSymbol);
    }
}