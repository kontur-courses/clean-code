using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string mdString) =>
            string.Join("", mdString
                .Split('\r', '\n')
                .Select(ParseParagraph)
                .Select(HtmlConverter.InsertNewTags));
        private static (string paragraph, List<ITag> tags) ParseParagraph(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static bool IsHeaderParagraph(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static List<(ITag start, ITag end)> GetPairedTags(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static string RemoveOldTags(string paragraph, List<ITag> tags)
        {
            throw new NotImplementedException();
        }

        private static string InsertNewTags(string paragraph, List<ITag> tags)
        {
            throw new NotImplementedException();
        }
    }
}
