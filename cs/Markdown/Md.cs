using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string md)
        {
            var result = new StringBuilder();
            foreach (var paragraph in md.Split('\n'))
            {
                var tags = ReadAllTags(paragraph)
                    .ShieldTags()
                    .GetPairedTags(md)
                    .RemoveTagsIntersection()
                    .RemoveStrongInItalics();
                result.Append($"{tags.ChangeMdTagsToHtml(paragraph)}\n");
            }

            return result.Remove(result.Length - 1, 1).ToString();
        }
        
        private static IEnumerable<Tag> ReadAllTags(string md)
        {
            throw new NotImplementedException();
        }
        
        private static TagType? ReadTagType(string md, int start, out int length)
        {
            length = 0;
            return null;
        }
        
        private static IEnumerable<Tag> ShieldTags(this IEnumerable<Tag> tags)
        {
            return tags;
        }
        
        private static IEnumerable<Tag> GetPairedTags(this IEnumerable<Tag> tags, string md)
        {
            return tags;
        }
        
        private static IEnumerable<Tag> RemoveTagsIntersection(this IEnumerable<Tag> tags)
        {
            return tags;
        }
        
        private static IEnumerable<Tag> RemoveStrongInItalics(this IEnumerable<Tag> tags)
        {
            return tags;
        }

        private static string ChangeMdTagsToHtml(this IEnumerable<Tag> tags, string md)
        {
            return md;
        }
    }
}