using System;
using Markdown.BasicTextTokenizer;

namespace Markdown
{
    public class BoldTagClassifier : ITagClassifier
    {
        public int Priority => 1;
        public FormattedTokenType Type => FormattedTokenType.Bold;
        public Type[] AllowedSubClassifiers => new []{typeof(ItalicTagClassifier)};

        public bool IsOpeningSequence(string text, int position)
        {
            if (position == 0)
                return position + 2 < text.Length
                       && text.Substring(position, 2) == "__"
                       && !char.IsWhiteSpace(text[position + 1]);

            return position + 2 < text.Length
                   && char.IsWhiteSpace(text[position - 1])
                   && text.Substring(position, 2) == "__"
                   && !char.IsWhiteSpace(text[position + 2]);
        }

        public bool IsClosingSequence(string text, int position)
        {
            if (position == text.Length - 2)
                return position > 0
                       && !char.IsWhiteSpace(text[position - 1])
                       && text.Substring(position, 2) == "__";
            return position + 2 < text.Length && position > 0
                                              && !char.IsWhiteSpace(text[position - 1])
                                              && text.Substring(position, 2) == "__"
                                              && char.IsWhiteSpace(text[position + 2]);
        }

    }
}
