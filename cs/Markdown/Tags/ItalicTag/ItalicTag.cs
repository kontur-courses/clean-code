namespace Markdown.Tags.ItalicTag
{
    public class ItalicTag : Tag
    {
        public const int TagLength = 1;
        public ItalicTag(string value, int index) : base(value, index, TagLength)
        {
        }
        
        public static bool IsTagEnd(string paragraph, int index)
        {
            return (paragraph[index] == '_'
                   && (index == paragraph.Length - 1 || paragraph[index + 1] != '_')
                   && paragraph[index - 1] != ' '
                   && paragraph[index - 1] != '_'
                   && paragraph[index - 1] != '\\')
                   || (paragraph[index] == '_' 
                       && index >= 2 
                       && paragraph[index - 1] == '_' 
                       && paragraph[index - 2] == '\\');
        }

        public static bool IsTagStart(string paragraph, int startIndex)
        {
            return (paragraph[startIndex] == '_'
                    && startIndex < paragraph.Length - 1
                    && !char.IsDigit(paragraph[startIndex + 1])
                    && paragraph[startIndex + 1] != ' '
                    && (startIndex == 0 || paragraph[startIndex - 1] != '_' 
                        && paragraph[startIndex - 1] != '\\')
                    && paragraph[startIndex + 1] != '_')
                   || (paragraph[startIndex] == '_' 
                       && startIndex >= 2 
                       && paragraph[startIndex - 1] == '_' 
                       && paragraph[startIndex - 2] == '\\');
        }
    }
}