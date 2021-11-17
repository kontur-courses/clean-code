using System.Collections.Generic;
using System.Text;
using Markdown.Interfaces;

namespace Markdown
{
    public class Tokenizer: IParser
    {
        private readonly StringBuilder valueTokenBuilder;
        private readonly List<TagToken> bufferTagTokens;

        public Tokenizer()
        {
            valueTokenBuilder = new StringBuilder();
            bufferTagTokens = new List<TagToken>();
        }

        public IEnumerable<TagToken> Parse(string expression)
        {
            return bufferTagTokens;
        }

        private TagToken CreateOperatorToken(string c)
        {
            return null;
        }

        private bool IsTagCharacter(char c)
        {
            return true;
        }

    }
}
