using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Tokenizer
    {
        public static IReadOnlyDictionary<T, IReadOnlyList<Token>> Tokenize<T>(
            string text,
            IEnumerable<ITag> tokenSeparators
        )
            where T : ITag
        {
            throw new NotImplementedException();
        }
    }
}