namespace Markdown.Tags.BoldTag
{
    public class BoldTag : Tag
    {
        public const int TagLength = 2;
        
        public BoldTag(string value, int index) : base(value, index, TagLength)
        {
            
        }
        
        public static bool IsTagStart(string paragraph, int startIndex)
        {
            return paragraph[startIndex] == '_'
                   && startIndex <= paragraph.Length - 3
                   && paragraph[startIndex + 1] == '_'
                   && !char.IsDigit(paragraph[startIndex + 2])
                   && (paragraph[startIndex + 2] != ' ')
                   && (startIndex == 0 || paragraph[startIndex - 1] != '\\');
        }

        public static bool IsTagEnd(string paragraph, int endIndex)
        {
            return paragraph[endIndex] == '_'
                   && endIndex < paragraph.Length - 1
                   && paragraph[endIndex + 1] == '_'
                   && paragraph[endIndex - 1] != ' '
                   && paragraph[endIndex - 1 ] != '\\';
        }
    }
    
}