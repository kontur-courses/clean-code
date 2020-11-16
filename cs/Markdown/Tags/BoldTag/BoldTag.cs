namespace Markdown.Tags.BoldTag
{
    public class BoldTag : Tag
    {
        public BoldTag(string value, int index) : base(value, index, 2)
        {
            
        }
        public static bool IsTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 3
                   && line[startIndex + 1] == '_'
                   && (line[startIndex + 2] != ' ')
                   && (startIndex == 0 || line[startIndex - 1] != '\\');
        }

        public static bool IsTokenEnd(string line, int endIndex)
        {
            return line[endIndex] == '_'
                   && endIndex < line.Length - 1
                   && line[endIndex + 1] == '_'
                   && line[endIndex - 1] != ' '
                   && line[endIndex - 1 ] != '\\';
        }
    }
    
}