using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public class Parser
    {
        private readonly ReadingOptions readingOptions;

        public Parser(ReadingOptions readingOptions)
        {
            this.readingOptions = readingOptions;
        }

        public List<IToken> Parse(string text)
        {
            var res = new List<IToken>();
            var curIdx = 0;

            while (curIdx < text.Length)
            {
                IToken token = null;
                foreach (var reader in readingOptions.AllowedReaders)
                {
                    int read;
                    (token, read) = reader.ReadToken(text, curIdx, readingOptions);
                    if (read == 0) continue;
                    res.Add(token);
                    curIdx += read;
                    break;
                }
                if (token == null)
                    throw new InvalidOperationException("Can't parse text with given reading options");
            }

            return res;
        }
    }
}