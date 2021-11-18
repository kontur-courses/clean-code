namespace Markdown.Tokens
{
    public abstract class StyleToken : Token
    {
        private bool isCorrect = true;

        public virtual bool IsCorrect
        {
            get => isCorrect;
            set
            {
                if (!isCorrect)
                    return;

                isCorrect = value;
            }
        }

        protected StyleToken(int openIndex) : base(openIndex) { }
        protected StyleToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }
    }
}