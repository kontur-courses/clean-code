namespace Markdown.Tags.BoldTag
{
    public class BoldTag : Tag
    {
        public new static int Length = 2;
        
        public BoldTag(string value, int index) : base(value, index, Length)
        {
            
        }
        
        public static bool IsTagStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 3
                   && line[startIndex + 1] == '_'
                   && !char.IsDigit(line[startIndex + 2])
                   && (line[startIndex + 2] != ' ')
                   && (startIndex == 0 || line[startIndex - 1] != '\\');
        }

        public static bool IsTagEnd(string line, int endIndex)
        {
            return line[endIndex] == '_'
                   && endIndex < line.Length - 1
                   && line[endIndex + 1] == '_'
                   && line[endIndex - 1] != ' '
                   && line[endIndex - 1 ] != '\\';
        }
    }
    
}