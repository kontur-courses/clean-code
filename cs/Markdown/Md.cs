using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        public readonly HashSet<string> dfsTags;
        private readonly Dictionary<string, Tag> tagFormats;

        public Md()
        {
            dfsTags = new HashSet<string>();
            tagFormats = new Dictionary<string, Tag>()
            {
                {new BoldText(this).Identifier, new BoldText(this)},
                {new ItalicText(this).Identifier, new ItalicText(this)},
                {new Title(this).Identifier, new Title(this)},
                {new Link(this).Identifier, new Link(this)},
            };
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentException("Text shouldn't be null");
            if (text == string.Empty)
                return text;
            var tokenizer = new Tokenizer(this, text).ParseToTokens();
            return Format(tokenizer.tokens.First?.Value);
        }

        private string Format(Token startToken)
        {
            if (startToken == null)
                return string.Empty;
            var next = startToken;
            var resultStr = new StringBuilder();
            while (next != null)
                resultStr.Append(FormatToken(ref next));
            return resultStr.ToString();
        }

        internal string FormatToken(ref Token token)
        {
            string result;
            if (token.Type == TokenType.Tag && !tagFormats.ContainsKey(token.Value))
            {
                result = token.Value;
                token = token.Next;
                return result;
            }

            if (token.Type == TokenType.Tag && !token.IgnoreAsTag)
            {
                dfsTags.Add(token.Value);
                result = tagFormats[token.Value].Format(token, out var newToken);
                dfsTags.Remove(token.Value);
                token = newToken;
            }
            else
            {
                result = token.Value;
            }
            token = token?.Next;
            return result;
        }

        public bool IsStartOfTag(char value)
        {
            return tagFormats.Keys.Any(t =>t.StartsWith(value));
        }

        public bool IsStartOfTag(string value)
        {
            return tagFormats.Keys.Any(t => t.StartsWith(value));
        }

        public Tag GetBySpecialSymbol(char symbol)
        {
            foreach (var kv in tagFormats)
            {
                if (kv.Value.SpecialSymbols.Contains(symbol))
                    return kv.Value;
            }
            return null;
        }

        public bool IsShieldSymbol(char value)
        {
            return value == '\\' || IsStartOfTag(value);
        }
    }
}
