using Markdown.Core.Tags;

namespace Markdown.Core
{
    static class TagValidator
    {
        public static bool IsPossibleClosingTag(string line, int index, ITag tag)
        {
            return (index == line.Length - tag.Closing.Length || char.IsWhiteSpace(line[index + tag.Closing.Length])) &&
                   (index == 0 || !char.IsWhiteSpace(line[index - 1]));
        }

        public static bool IsPossibleOpenningTag(string line, int index, ITag tag)
        {
            return (index == 0 || char.IsWhiteSpace(line[index - 1])) &&
                   (index == line.Length - tag.Opening.Length || !char.IsWhiteSpace(line[index + tag.Opening.Length]));
        }
    }
}