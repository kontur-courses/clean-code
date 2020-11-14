using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        private readonly Dictionary<string, Tag> handlers = new Dictionary<string, Tag>();
        private readonly HashSet<string> currentTags = new HashSet<string>();

        public Md()
        {
            AddHandler(new OneUnderscore(this));
            AddHandler(new DoubleUnderscore(this));
            AddHandler(new Title(this, 1));
            AddHandler(new Title(this, 2));
            AddHandler(new Title(this, 3));
            AddHandler(new Title(this, 4));
            AddHandler(new Title(this, 5));
            AddHandler(new Title(this, 6));
        }

        private void AddHandler(Tag tag)
        {
            handlers.Add(tag.Identifier, tag);
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentException("Text should not be null");
            var tokenizer = new Tokenizer(text);
            return Format(tokenizer.First);
        }

        internal string Format(Token token)
        {
            if (token == null)
                return string.Empty;
            var next = token;
            var result = new StringBuilder();
            while (next != null)
                result.Append(FormatToken(ref next));
            return result.ToString();
        }

        internal string FormatToken(ref Token token)
        {
            string result;
            if (ContainsHandler(token))
            {
                var tagToken = token;
                currentTags.Add(tagToken);
                result = handlers[token].Format(token, out token);
                currentTags.Remove(tagToken);
            }
            else
                result = token;
            token = token?.Next;
            return result;
        }

        internal bool IAmIn(string identifier)
        {
            return currentTags.Contains(identifier);
        }

        internal bool ContainsHandler(string identifier)
        {
            return handlers.ContainsKey(identifier);
        }
    }
}
