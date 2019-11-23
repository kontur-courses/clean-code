using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.BasicTextTokenizer
{
    public class LinkUriTagClassifier : ITagClassifier
    {
        public bool IsOpeningSequence(string text, int position)
        {
            return position < text.Length && text[position] == '(';
        }

        public bool IsClosingSequence(string text, int position)
        {
            return position < text.Length && text[position] == ')';
        }

        public int Priority => 4;
        public FormattedTokenType Type => FormattedTokenType.LinkUri;
        public Type[] AllowedSubClassifiers => null;
        public int TagLength => 1;
        public bool HasSecondPart => false;
        public bool HasFirstPart => true;
        public Type SecondPartType => null;
    }
}
