using System.Linq;

namespace Markdown
{
    class TextToken : SingleToken
    {
        public bool WithDigits { get; init; }
        protected override string Value => value;
        private readonly string value;

        public TextToken(string value)
        {
            this.value = value;
            WithDigits = value.Any(char.IsDigit);
        }
    }
}
