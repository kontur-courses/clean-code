using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class TokenWriter
    {
        private StringBuilder stringBuilder;
        private int indexOffset;

        public TokenWriter(string line)
        {
            stringBuilder = new StringBuilder(line);
        }

        public void Write(Token token)
        {
            token.ReplaceOpeningTag(stringBuilder, ref indexOffset);

            foreach (var subToken in token.SubTokens)
            {
                Write(subToken);
            }

            token.ReplaceClosingTag(stringBuilder, ref indexOffset);
        }

        public string GetString() => stringBuilder.ToString();
    }
}
