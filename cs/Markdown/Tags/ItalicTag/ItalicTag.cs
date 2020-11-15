using System;

namespace Markdown
{
    public class ItalicTag : Tag
    {
        public ItalicTag(string value, int index) : base(value, index, 1)
        {
        }
        public static bool IsTokenEnd(string line, int index)
        {
            return line[index] == '_' && line[index - 1] != ' '
                                      && line[index - 1] != '\\'
                                      && (index == line.Length - 1 || line[index + 1] != '_' || line[index - 1] == '\\')
                                      && line[index - 1] != '_';
        }

        public static bool IsTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex != line.Length - 1
                   && (line[startIndex + 1] != '_' || startIndex > 0 && line[startIndex - 1] == '\\')
                   && !Char.IsDigit(line[startIndex + 1])
                   && line[startIndex + 1] != ' '
                   && (startIndex == 0 || line[startIndex - 1] != '_' && line[startIndex - 1] != '\\') ;
        }
    }
}