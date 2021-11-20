using System;

namespace Markdown.Models
{
    public class Context
    {
        public string Text { get; }
        public int Index { get; set; }

        public Context(string text, int index = 0)
        {
            Text = text ?? throw new ArgumentException($"{nameof(text)} can't be null", nameof(text));
            Index = index;
        }
    }
}