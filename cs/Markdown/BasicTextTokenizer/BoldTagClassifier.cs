using System;

namespace Markdown.BasicTextTokenizer
{
    public class BoldTagClassifier : ITagClassifier
    {
        public int Priority => 1;
        public FormattedTokenType Type => FormattedTokenType.Bold;
        public Type[] AllowedSubClassifiers => new[] { typeof(ItalicTagClassifier), typeof(LinkTextTagClassifier) };
        public int TagLength => 2;
        public bool HasSecondPart => false;
        public bool HasFirstPart => false;
        public Type SecondPartType => null;
        private const string Tag = "__";

        public bool IsOpeningSequence(string text, int position)
        {
            return position + 2 < text.Length
                   && text.Substring(position, 2) == Tag
                   && !char.IsWhiteSpace(text[position + 2]);
        }

        public bool IsClosingSequence(string text, int position)
        {
            return position > 0 && position + 1 < text.Length
                   && !char.IsWhiteSpace(text[position - 1])
                   && text.Substring(position, 2) == Tag;
        }

    }
}
