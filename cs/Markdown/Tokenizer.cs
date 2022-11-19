using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly List<Token> tokens;

        private readonly Md markdown;
        private readonly string text;

        public Token First
        {
            get => tokens[0];
        }

        public Tokenizer(Md markdown, string text)
        {
            tokens = new List<Token>();
            this.text = text;
            this.markdown = markdown;
        }

        public Tokenizer ParseToTokens()
        {
            //TODO
            //list = parsed tokens from text
            //для уточнения: последовательность символов одного типа TokenType будет токеном
            return this;
        }
    }
}
