using System;

namespace Markdown.BasicTextTokenizer
{
    public class ItalicTagClassifier : ITagClassifier
    {
        public int Priority => 2;
        public FormattedTokenType Type => FormattedTokenType.Italic;
        public Type[] AllowedSubClassifiers => null;
        public string Tag => "_";

        public bool IsOpeningSequence(string text, int position)
        {
            return position + 1 < text.Length 
                   && text[position] == '_'
                   && !IsWhiteSpaceOrUnderscore(text[position + 1]);
        }

        public bool IsClosingSequence(string text, int position)
        {
            return position > 0 && position < text.Length
                                && !IsWhiteSpaceOrUnderscore(text[position - 1])
                                && text[position] == '_'
                                && (!NextSymbolIsUnderscore(text, position) 
                                    || IsFollowedByDoubleUnderscore(text, position));
                
        }

        private bool IsWhiteSpaceOrUnderscore(char symbol)
        {
            return char.IsWhiteSpace(symbol) || symbol == '_';
        }

        private bool NextSymbolIsUnderscore(string text, int position)
        {
            return position + 1 < text.Length && text[position + 1] == '_';
        }

        private bool IsFollowedByDoubleUnderscore(string text, int position)
        {
            return position + 2 < text.Length && text.Substring(position + 1, 2) == "__";
        }
    }
}
