using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class TagReader : IReader
    {
        private IEnumerable<IReader> readers;
        private IEnumerable<TagReader> skippedReaders;
        private string mdTag;
        private string htmlTag;

        public TagReader(string mdTag, string htmlTag,
            IEnumerable<IReader> readers,
            IEnumerable<TagReader> skippedReaders)
        {
            this.mdTag = mdTag;
            this.readers = readers;
            this.htmlTag = htmlTag;
            this.skippedReaders = skippedReaders;
        }

        private bool IsOpenTag(string text, int position)
        {
            return position < text.Length - mdTag.Length - 1  &&
                    CanHandle(text, position) &&
                    !char.IsWhiteSpace(text[position + mdTag.Length]) &&
                   (text.Substring(position + mdTag.Length, mdTag.Length) != mdTag) &&
                    !char.IsDigit(text[position + mdTag.Length]);
        }

        public bool CanHandle(string text, int position)
        {
            return position <= text.Length - mdTag.Length &&
                   text.Substring(position, mdTag.Length) == mdTag;
        }

        private bool IsClosedTag(string text, int position)
        {
            return CanHandle(text, position) && 
                   !char.IsWhiteSpace(text[position - 1]);
        }

        private IToken GetToken(string text, int index)
        {
            return readers.Select(reader => reader.ReadToken(text, index))
                .FirstOrDefault(token => token != null);
        }

        public IToken ReadToken(string text, int position)
        {
            if (!IsOpenTag(text, position))
                return null;
            var tokens = new List<IToken>();

            for (int i = position + mdTag.Length; i <= text.Length - mdTag.Length; i++)
            {
                IToken token = GetSkippedToken(text, i);
                if (string.IsNullOrEmpty(token.Text))
                {

                    if (IsClosedTag(text, i))
                    {
                        return new Tag(text.Substring(position, i - position + mdTag.Length), htmlTag, tokens);
                    }

                    token = GetToken(text, i);
                }

                tokens.Add(token);
                i += token.Text.Length - 1;
            }

            return null;
        }

        private IToken GetSkippedToken(string text, int i)
        {
            var maxTokenTagLength = skippedReaders.Where(reader => reader.CanHandle(text, i))
                .Select(reader => reader.mdTag.Length).Concat(new []{0}).Max();
            return new Token(text.Substring(i, maxTokenTagLength));
        }
    }
}
