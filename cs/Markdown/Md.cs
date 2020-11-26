using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        private readonly HashSet<string> currentTags = new HashSet<string>();
        private readonly Dictionary<string, Tag> handlers = new Dictionary<string, Tag>();
        private readonly CharTree identifiers = new CharTree();

        public Md()
        {
            AddHandler(new OneUnderscore(this));
            AddHandler(new DoubleUnderscore(this));
            AddHandler(new ListTag(this, "*"));
            for (var i = 1; i <= 6; i++)
                AddHandler(new Title(this, i));
        }

        internal Tag this[string identifier] => handlers[identifier];

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentException("Text should not be null");
            var tokenizer = new Tokenizer(this, text);
            return Format(tokenizer.First);
        }

        public bool IsShieldSymbol(char value)
        {
            return value == '\\' || IsStartOfTag(value);
        }

        internal string FormatToken(ref Token token)
        {
            string result;
            if (token.Type == TokenType.Tag && !token.IgnoreAsTag)
            {
                var tagToken = token;
                currentTags.Add(tagToken);
                result = handlers[token].Format(token, out token);
                currentTags.Remove(tagToken);
            }
            else
            {
                result = token;
            }

            token = token?.Next;
            return result;
        }

        internal bool ContainsTag(string identifier, out int depth)
        {
            depth = identifiers.SearchDepth(identifier, out _);
            return depth == identifier.Length && handlers.ContainsKey(identifier);
        }

        internal bool IsStartOfTag(char value)
        {
            return identifiers.SearchDepth(value.ToString(), out _) == 1;
        }

        internal bool IsIn(string identifier)
        {
            return currentTags.Contains(identifier);
        }

        private string Format(Token token)
        {
            if (token == null)
                return string.Empty;
            var next = token;
            var result = new StringBuilder();
            while (next != null)
                result.Append(FormatToken(ref next));
            return result.ToString();
        }

        private void AddHandler(Tag tag)
        {
            identifiers.Add(tag.Identifier);
            handlers.Add(tag.Identifier, tag);
        }
    }
}
