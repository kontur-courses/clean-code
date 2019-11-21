using System;
using Markdown.BasicTextTokenizer;

namespace Markdown
{
    public class ItalicTagClassifier : ITagClassifier
    {
        public int Priority => 2;
        public FormattedTokenType Type => FormattedTokenType.Italic;
        public Type[] AllowedSubClassifiers => null;

        public bool IsOpeningSequence(string text, int position)
        {
            if (position == 0)
                return position + 1 < text.Length
                       && text[position] == '_'
                       && !IsWhiteSpaceOrUnderscore(text[position + 1]);

            return position + 1 < text.Length
                   && char.IsWhiteSpace(text[position - 1])
                   && text[position] == '_'
                   && !IsWhiteSpaceOrUnderscore(text[position + 1]);
        }

        public bool IsClosingSequence(string text, int position)
        {
            if (position == text.Length - 1)
                return position > 0
                       && !IsWhiteSpaceOrUnderscore(text[position - 1])
                       && text[position] == '_';

            return position + 1 < text.Length && position > 0
                                              && !IsWhiteSpaceOrUnderscore(text[position - 1])
                                              && text[position] == '_'
                                              && char.IsWhiteSpace(text[position + 1]);
        }

        private bool IsWhiteSpaceOrUnderscore(char symbol)
        {
            return char.IsWhiteSpace(symbol) || symbol == '_';
        }
    }
}
