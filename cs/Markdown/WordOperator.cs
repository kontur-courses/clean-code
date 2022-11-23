using Markdown.TagClasses.ITagInterfaces;

namespace Markdown
{
    public class WordOperator
    {
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

        public static bool IsWordContainsDigits(string word) => word.Any(x => x is >= '0' and <= '9');

        public static bool InDifferentWords(string paragraph, IPairedTag start, IPairedTag end)
        {
            var length = end.Position - start.Position;
            var word = paragraph.Substring(start.Position, length);
            return word.Contains(' ');
        }
    }
}