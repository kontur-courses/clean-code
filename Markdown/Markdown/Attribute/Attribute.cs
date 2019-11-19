using System;

namespace Markdown
{
    public class Attribute
    {
        public AttributeType Type;

        private readonly Func<string, int, bool> charValidationFunc;

        public Attribute(AttributeType type, Func<string, int, bool> charValidationFunc)
        {
            Type = type;
            this.charValidationFunc = charValidationFunc;
        }

        public bool IsCharValid(string source, int charPosition)
        {
            return charValidationFunc(source, charPosition);
        }
    }
}