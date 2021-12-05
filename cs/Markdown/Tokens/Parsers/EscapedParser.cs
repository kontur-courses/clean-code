using System.Collections.Generic;

namespace Markdown.Tokens.Parsers
{
    public class EscapedParser: ITokenParser
    {
        private readonly HashSet<char> markingSymbols = new()
        {
            '_',
            '#',
            '[',
            ']',
            '(',
            ')',
            '\n',
            '\\'
        };
        
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == '\\' && NextCharIsMarking(rawString, position))
            {
                token = new CharToken(rawString[position + 1]);
                position += 2;
                return true;
            }
            
            token = default;
            return false;
        }

        private bool NextCharIsMarking(string rawString, int position)
        {
            if (!rawString.InBorders(position + 1))
                return false;
            var nextChar = rawString[position + 1];
            return markingSymbols.Contains(nextChar);
        }
    }
}