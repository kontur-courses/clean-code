using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public abstract class Text : IReadingState
    {
        protected readonly StringBuilder Content;
        protected char PreviousSymbol;
        
        private bool isEscape;
        
        protected Text()
        {
            Content = new StringBuilder();
        }

        protected Text(string initialText)
        {
            Content.Append(initialText);
        }

        public IReadingState ProcessSymbol(char symbol)
        {
            switch (symbol)
            {
                case '\\':
                    return ProcessBackSlash();
                case '_':
                    if (!isEscape) return ProcessUnderscore();
                    Content.Append(symbol);
                    isEscape = false;
                    return this;
                default:
                    return ProcessDefault(symbol);
            }
        }

        protected IReadingState ProcessBackSlash()
        {
            isEscape = true;
            return this;
        }

        public void AddContent(StringBuilder content)
        {
            Content.Append(content);
        }

        protected abstract IReadingState ProcessUnderscore();

        protected abstract IReadingState ProcessDefault(char symbol);

        public abstract IEnumerable<Token> GetContentTokens();
    }
}
