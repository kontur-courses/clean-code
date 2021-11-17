using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Tokenizer
    {
        public static IReadOnlyDictionary<ITag, IReadOnlyList<Token>> Tokenize(
            string text,
            IEnumerable<ITag> tokenSeparators
        )
        {
            throw new NotImplementedException();
        }
    }
}