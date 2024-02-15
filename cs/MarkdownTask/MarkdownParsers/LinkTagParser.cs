using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownParsers
{
    public class LinkTagParser : IMarkdownParser
    {
        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();
            var startIndex = 0;

            while ((startIndex = markdown.IndexOf("[", startIndex)) != -1)
            {
                var sequenceLength = CorrectBracketsSequenceLength(markdown, startIndex);

                if (sequenceLength != -1)
                {
                    tokens.Add(new Token(TagType.Link, startIndex, Tag.Open, sequenceLength));
                    tokens.Add(new Token(TagType.Link, startIndex + sequenceLength - 1, Tag.Close, 0));

                    startIndex += sequenceLength;
                }
                else
                {
                    startIndex++;
                }
            }

            return tokens;
        }

        private int CorrectBracketsSequenceLength(string text, int startIndex)
        {
            string sequence = "]()";

            if (startIndex == -1)
            {
                return -1;
            }

            var bracketsPosition = new List<int>() { startIndex };

            foreach (char c in sequence)
            {
                var bracketIndex = text.IndexOf(c, bracketsPosition.Last());

                if (bracketIndex == -1)
                {
                    return -1;
                }

                bracketsPosition.Add(bracketIndex);
            }

            if (bracketsPosition[2] != bracketsPosition[1] + 1)
                return -1;

            return bracketsPosition.Last() - bracketsPosition.First() + 1;
        }
    }
}
