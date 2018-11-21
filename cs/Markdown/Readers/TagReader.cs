using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Markdown.StringExtension;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class TagReader : IReader
    {
        private readonly string mdTag;
        private readonly IEnumerable<IReader> readers;
        private readonly IEnumerable<TagReader> skippedReaders;
        private readonly List<IToken> innerTokens = new List<IToken>();

        public TagReader(string mdTag,
            IEnumerable<IReader> readers,
            IEnumerable<TagReader> skippedReaders)
        {
            this.mdTag = mdTag;
            this.readers = readers;
            this.skippedReaders = skippedReaders;
        }

        public IToken ReadToken(string text, int position)
        {
            if (!IsOpenTag(text, position))
                return null;

            for (var i = position + mdTag.Length; i <= text.Length - mdTag.Length; i++)
            {
                if (IsClosedTag(text, i))
                {
                    var rightPosition = innerTokens.Last().Position;

                    return new TagToken(text.Substring(position, i - position + mdTag.Length), mdTag, innerTokens,
                        rightPosition + mdTag.Length);
                }
                
                if (IsOpenTag(text, i))
                {
                    break;
                }
                
                var token = GetSkippedToken(text, i);
                if (string.IsNullOrEmpty(token.Text))
                    token = GetToken(text, i);

                if (token.Text.Any(char.IsDigit)) break;
                innerTokens.Add(token);
                i = token.Position;
            }

            return null;
        }

        private bool IsOpenTag(string text, int position)
        {
            var nextTagIsNotCurrent = text.CompareWithSubstring(position + mdTag.Length, mdTag);

            return nextTagIsNotCurrent &&
                   CanReadTag(text, position) &&
                   IsLetterOrSlash(text[position + mdTag.Length]);
        }

        private bool IsLetterOrSlash(char symbol)
        {
            return !char.IsWhiteSpace(symbol) || symbol == '\\';
        }

        private bool CanReadTag(string text, int position)
        {
            return position <= text.Length - mdTag.Length &&
                   text.Substring(position, mdTag.Length) == mdTag;
        }

        private bool IsClosedTag(string text, int position)
        {
            var  nextTagIsNotOpen = !IsOpenTag(text, position + mdTag.Length);
            
            return CanReadTag(text, position) &&
                   !char.IsWhiteSpace(text[position - 1]) &&
                   nextTagIsNotOpen &&
                   innerTokens.Count != 0;
        }

        private IToken GetToken(string text, int index)
        {
            return readers.Select(reader => reader.ReadToken(text, index))
                .FirstOrDefault(token => token != null);
        }

        private IToken GetSkippedToken(string text, int position)
        {
            var maxTokenTagLength = skippedReaders.Where(reader => reader.CanReadTag(text, position))
                .Select(reader => reader.mdTag.Length).DefaultIfEmpty(0).Max();
            
            return new TextToken(text.Substring(position, maxTokenTagLength), position + maxTokenTagLength - 1);
        }
    }
}