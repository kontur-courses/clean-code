using System;

namespace Markdown.Analyzers
{
    public class WordsContainingDigitsAnalyzer
    {
        public static bool[] Analyze(string text)
        {
            bool[] isInsideWordWithDigits = new bool[text.Length];
            int currentPosition = 0;

            while(currentPosition < text.Length)
            {
                var nextWordPosition = text.GetIndexOfFirstMatching(currentPosition,
                    ch => !Char.IsWhiteSpace(ch));
                FillSegmentWith(isInsideWordWithDigits, currentPosition, nextWordPosition, false);

                var positionAfterWord = text.GetIndexOfFirstMatching(nextWordPosition,
                    ch => Char.IsWhiteSpace(ch));
                var wordContainsDigits = text.ContainsMatchingSymbolsBetween(nextWordPosition, positionAfterWord,
                    ch => Char.IsDigit(ch));
                FillSegmentWith(isInsideWordWithDigits, nextWordPosition, positionAfterWord, wordContainsDigits);

                currentPosition = positionAfterWord;
            }

            return isInsideWordWithDigits;
        }

        private static void FillSegmentWith(bool[] array, int segmentLeft, int segmentRight, bool value)
        {
            for (var index = segmentLeft; index < segmentRight; index++)
                array[index] = value;
        }
    }
}
