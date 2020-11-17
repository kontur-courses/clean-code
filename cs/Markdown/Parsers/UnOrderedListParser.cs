using System.Linq;
using Markdown.Tags;
using Markdown.Tags.ListIemTag;
using Markdown.Tags.UnorderedListTag;

namespace Markdown.Parsers
{
    public static class UnOrderedListParser
    {
        private static bool isUnOrderedListStart;
        
        public static Tag[] ParseTags(string paragraph)
        {
            if (paragraph[0] == '*' && paragraph[1] == ' ')
            {
                if (isUnOrderedListStart)
                {
                    return GetListItemTags(paragraph);
                }
                isUnOrderedListStart = true;
                return new Tag[]{new OpenUnOrderedListTag() }
                    .Concat(GetListItemTags(paragraph))
                    .ToArray();
            }
            if (isUnOrderedListStart)
            {
                isUnOrderedListStart = false;
                return new Tag[]{new CloseUnOrderedListTag() };
            }

            return new Tag[0];
        }
        
        private static Tag[] GetListItemTags(string paragraph)
        {
            return new Tag[] {new OpenListItemTag(0), new CloseListItemTag(paragraph.Length)};
        }
    }
}