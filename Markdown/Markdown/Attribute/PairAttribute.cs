using System;

namespace Markdown
{
    public class PairAttribute : Attribute
    {
        private readonly Func<string, int, bool> isCharClosingFunction;

        public PairAttribute(AttributeType type, Func<string, int, bool> charValidationFunc,
            Func<string, int, bool> isCharClosingFunction) : base(type, charValidationFunc)
        {
            this.isCharClosingFunction = isCharClosingFunction;
        }

        public bool IsCharClosing(string source, int charPosition)
        {
            return isCharClosingFunction(source, charPosition);
        }
    }
}