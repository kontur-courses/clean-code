using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TagStore;

namespace Markdown.Tokens
{
    public class Tokenizer : ITokenizer
    {
        private static readonly HashSet<string> Escaping = new() { "\\" };
        private readonly HashSet<string> allTags;
        private readonly ITagStore store;
        private List<Token> tokens;
        private (int start, int length)[] tags;
        private string text;

        public Tokenizer(ITagStore store)
        {
            this.store = store;
            allTags = store.GetTagsValues();
        }

        public IEnumerable<Token> Tokenize(string text)
        {
            this.text = text;
            tokens = new List<Token>();

            tags = text.FindAll(allTags.Concat(Escaping))
                .OrderBy(s => s.start).ToArray();

            AddStartText();

            for (var i = 0; i < tags.Length; i++)
            {
                if (i > 0)
                {
                    AddTextBetween(tags[i - 1], tags[i]);
                }

                if (text[tags[i].start] == '\\')
                {
                    tokens.Add(new Token(TokenType.Escape, tags[i].start, 1));
                    continue;
                }

                AddToken(tags[i].start, tags[i].length);
            }

            AddLastText();

            return tokens.RemoveEscaping().RemoveUnpaired(text).OrderBy(t => t.Start);
        }

        private void AddTextBetween((int start, int length) firstTag, (int start, int length) secondTag)
        {
            var textStart = firstTag.start + firstTag.length;
            var textLength = secondTag.start - textStart;
            if (textLength > 0)
                tokens.Add(new Token(TokenType.Text, textStart, textLength));
        }

        private void AddToken(int tagStart, int tagLength)
        {
            var tagType = store.GetTagType(text, tagStart, tagLength);
            var tagRole = store.GetTagRole(text, tagStart, tagLength);
            var token = new Token(TokenType.Tag, tagType, tagRole, tagStart, tagLength);
            tokens.Add(token);
        }

        private void AddStartText()
        {
            if (tags[0].start == 0) return;
            var textToken = new Token(TokenType.Text, 0, tags[0].start);
            tokens.Add(textToken);
        }

        private void AddLastText()
        {
            if (tags[^1].start + tags[^1].length == text.Length) return;
            var textStart = tags[^1].start + tags[^1].length;
            var textToken = new Token(TokenType.Text, textStart, text.Length - textStart);
            tokens.Add(textToken);
        }
    }
}