namespace Markdown
{
    public class BoldTag : Tag
    {
        public BoldTag(string value, int index) : base(value, index, 2)
        {
            
        }
        public static bool IsTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 2
                   && line[startIndex + 1] == '_'
                   && (startIndex == line.Length - 2 || line[startIndex + 2] != ' ')
                   && (startIndex == 0 || line[startIndex - 1] != '\\');
        }

        public static bool IsTokenEnd(string line, int endIndex)
        {
            return line[endIndex] == '_' && line[endIndex + 1] == '_' && 
                   (endIndex < 1 || line[endIndex - 1] != ' ');
        }
    }
    
    public class OpenBoldTag : BoldTag
    {
        public OpenBoldTag( int index) : base("<strong>", index)
        {
        }
    }
    public class CloseBoldTag : BoldTag
    {
        public CloseBoldTag(int index) : base("</strong>", index)
        {
        }
    }
}