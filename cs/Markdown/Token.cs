using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public int Length => Text.Length;
        public string Text { get; }

        public bool IsEmpty => Length == 0;
        public bool IsInterior { get; }


        public Token(int position, string text, bool isInterior)
        {
            Position = position;
            Text = text;
            IsInterior = isInterior;
        }
    }
}