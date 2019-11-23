using System;

namespace Markdown.BasicTextTokenizer
{
    public class LinkTextTagClassifier : ITagClassifier
    {
        public bool IsOpeningSequence(string text, int position)
        {
            return position < text.Length && text[position] == '[';
        }

        public bool IsClosingSequence(string text, int position)
        {
            return position < text.Length && text[position] == ']';
        }

        public int Priority => 3;
        public FormattedTokenType Type => FormattedTokenType.LinkText;
        public Type[] AllowedSubClassifiers => null;
        public int TagLength => 1;
        public bool HasSecondPart => true;
        public bool HasFirstPart => false;
        public Type SecondPartType => typeof(LinkUriTagClassifier);
    }
}
