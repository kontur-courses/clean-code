using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.TagStore;

namespace Markdown.Tokens
{
    public class Tokenizer : ITokenizer
    {
        private readonly ITagStore store;
        private string[] tags;

        public Tokenizer(ITagStore store)
        {
            this.store = store;
            tags = store.GetTagsValues();
        }

        public IEnumerable<Token> Tokenize(string text)
        {
            var tokens = new Stack<Token>();
            var allTagsInText = text.FindAll(tags).OrderBy(i => i.start);
            var escapeIndex = -1;
            foreach (var (start, tagLength) in allTagsInText)
            {
                if (start == escapeIndex)
                {
                    yield return new Token(TagType.Escaped, start - 1, 1, TagRole.NotTag, TokenType.Tag);
                    continue;
                }

                var tagType = store.GetTagType(text, start, tagLength);
                if (tagType == TagType.Escaped)
                {
                    escapeIndex = start + tagLength;
                    continue;
                }

                var tagRole = store.GetTagRole(text, start, tagLength);
                var token = new Token(tagType, start, tagLength, tagRole, TokenType.Tag);
                switch (tagRole)
                {
                    case TagRole.Opening:
                        tokens.Push(token);
                        break;
                    case TagRole.Closing:
                        if (tokens.Count != 0 && tokens.Peek().Type == tagType)
                        {
                            yield return tokens.Pop();
                            yield return token;
                        }

                        break;
                }
            }
        }
    }
}