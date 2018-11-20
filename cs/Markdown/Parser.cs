using System;
using System.Collections.Generic;
using System.Linq;
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
                var (token, read) = readingOptions.AllowedReaders
                    .Select(reader => reader.ReadToken(text, curIdx, readingOptions))
                    .FirstOrDefault(readingResult => readingResult.token != null);
                if (token == null)
                    throw new InvalidOperationException("Can't parse text with given reading options");
                res.Add(token);
                curIdx += read;
            }

            return res;
        }
    }
}