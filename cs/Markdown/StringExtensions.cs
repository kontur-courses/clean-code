namespace Markdown
{
    public static class StringExtensions
    {
        public static string Replace(this string text, string textToInsert, int index, int length)
        {
            return text
                .Remove(index, length)
                .Insert(index, textToInsert);
        }
    }
}