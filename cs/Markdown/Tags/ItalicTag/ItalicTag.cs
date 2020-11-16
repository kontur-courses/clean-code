namespace Markdown.Tags.ItalicTag
{
    public class ItalicTag : Tag
    {
        public new static int Length = 1;
        public ItalicTag(string value, int index) : base(value, index, Length)
        {
        }
        
        public static bool IsTagEnd(string line, int index)
        {
            return (line[index] == '_'
                   && (index == line.Length - 1 || line[index + 1] != '_')
                   && line[index - 1] != ' '
                   && line[index - 1] != '_'
                   && line[index - 1] != '\\')
                   || (line[index] == '_' 
                       && index >= 2 
                       && line[index - 1] == '_' 
                       && line[index - 2] == '\\');
        }

        public static bool IsTagStart(string line, int startIndex)
        {
            return (line[startIndex] == '_'
                    && startIndex < line.Length - 1
                    && !char.IsDigit(line[startIndex + 1])
                    && line[startIndex + 1] != ' '
                    && (startIndex == 0 || line[startIndex - 1] != '_' 
                        && line[startIndex - 1] != '\\')
                    && line[startIndex + 1] != '_')
                   || (line[startIndex] == '_' 
                       && startIndex >= 2 
                       && line[startIndex - 1] == '_' 
                       && line[startIndex - 2] == '\\');
        }
    }
}