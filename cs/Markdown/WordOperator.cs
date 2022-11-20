using Markdown.TagClasses;

namespace Markdown
{
    internal class WordOperator
    {
        private static readonly char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static string GetWordAtPosition(string paragraph, int position)
        {
            var firstHalf = paragraph[..position];
            var secondHalf = paragraph[position..];
            if (firstHalf.Contains(' '))
                firstHalf = firstHalf[firstHalf.LastIndexOf(' ')..];
            if (secondHalf.Contains(' '))
                secondHalf = secondHalf[..secondHalf.IndexOf(' ')];
            return (firstHalf + secondHalf).Trim();
        }

        public static bool IsWordContainsDigits(string word) => digits.Any(x => word.Contains(x));

        public static bool InDifferentWords(string paragraph, TagInfo start, TagInfo end)
        {
            var length = end.Position - start.Position;
            var word = paragraph.Substring(start.Position, length);
            return word.Contains(' ');
        }
    }
}