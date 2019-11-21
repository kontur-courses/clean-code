using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenConverters
{
    public abstract class TokenConverter
    {
        protected abstract IReadOnlyDictionary<TagType, (string start, string end)> TagAssociationsWithType { get; }

        public abstract string FileType { get; }
        public string FileTypeWithDot => '.' + FileType;

        public abstract string ConvertTokens(IEnumerable<Token> tokens);
    }
}