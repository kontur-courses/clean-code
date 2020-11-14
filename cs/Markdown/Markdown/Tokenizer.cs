using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        private string[] tags = {"_", "__", "#",};

        public string[] GetWords(string text)
        {
            return text.Split(' ');
        }

        public int ReadUntil(string text, string stopString)
        {
            var index = text.IndexOf(stopString);

            return index == -1 ? text.Length : index;
        }

        public int ReadUntil(string text, char[] symbols)
        {
            var firstIndex = symbols
                .Select(symbol => text.IndexOf(symbol))
                .Where(symbolIndex => symbolIndex != -1)
                .Prepend(text.Length)
                .Min();
            return firstIndex;
        }

        public Token ReadToken(string text)
        {
            if (text == "")
                return new Token(0, 0);

            var words = GetWords(text);
            var markup = new Stack<MarkupType>();
            if (IsFlatWord(words[0]))
                return new Token(0, 1);
            markup.Push(GetTag(words[0]));
            var token = new Token(0, 0);
            while (markup.Any())
            {
            }

            throw new NotImplementedException();
            return token;
        }

        private bool IsFlatWord(string word)
        {
            throw new NotImplementedException();
            return tags.All(tag => !word.StartsWith(tag));
        }
    }
}