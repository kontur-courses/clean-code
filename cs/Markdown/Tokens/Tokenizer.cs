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

            foreach (var (tagValue, index) in text.FindAll(tags))
            {
                char? charBefore = index > 0 ? text[index - 1] : null;
                char? charAfter = index < text.Length ? text[index + tagValue.Length] : null;
                var tagType = store.GetTagType(tagValue);
                var tagRole = store.GetTagRole(tagValue, charBefore, charAfter);

                var token =  new Token(tagType, index, tagValue.Length, tagRole);
                switch (tagRole)
                {
                    case TagRole.Opening:
                        tokens.Push(token);
                        break;
                    case TagRole.Closing:
                        if (tokens.Count == 0) break;
                        else if (tokens.Peek().Type == tagType)
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