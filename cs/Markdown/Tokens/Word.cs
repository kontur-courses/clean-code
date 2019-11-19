namespace Markdown.Tokens
{
    internal class Word : Token
    {
        private string text;

        public Word(string text)
        {
            this.text = text;
        }

        public override string ToText() => text;
    }
}
