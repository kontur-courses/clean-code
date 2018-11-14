using System;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1
{
    public class TextPart
    {
        public readonly string Text;
        public readonly TextType Type;
        
        public TextPart(string text, TextType type)
        {
            if (text == null)
                throw new ArgumentException("Text can't be null");
            Text = text;
            Type = type;
        }
    }
}
