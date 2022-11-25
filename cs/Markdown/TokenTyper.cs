using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Exstensions;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenTyper:ITokenTyper<TokenType>
    {

        private readonly HashSet<char> serviceSymbols = new HashSet<char>() { '\\', '_', '#' };

        private readonly string line;

        private readonly ITagCondition<TokenType> tagCondition;
        public string Line => line.ToString();

        public TokenTyper(string line, ITagCondition<TokenType> condition)
        {
            this.line = line;
            tagCondition = condition;
        }
        public TokenType GetSymbolType(int index)
        {
            switch (line[index])
            {
                case '_':
                    if (IsDoubleUnderscore(line, index))
                        return TokenType.Strong;
                    if (IsUnderscore(line, index))
                        return TokenType.Italic;
                    break;
                case '\\':
                    if (index + 1 < line.Length && serviceSymbols.Contains(line[index + 1]))
                        return TokenType.Slash;
                    break;
                case '#':
                    if (index == 0)
                        return TokenType.Header;
                    break;
            }
            return TokenType.Text;
        }

        private bool IsDoubleUnderscore(string line, int index)
        {
            if ((line.HasElementAt(index+1) && line[index + 1] != '_') || tagCondition.GetTagOpenStatus(TokenType.Italic))
                return false;

            return tagCondition.GetTagOpenStatus(TokenType.Strong) && line.IsCloseTag(index) || !tagCondition.GetTagOpenStatus(TokenType.Strong) && line.IsOpenTag(index + 1);
        }

        private bool IsUnderscore(string line, int index)
        {
            if (tagCondition.GetTagOpenStatus(TokenType.Italic))
            {
                return (line.CharInMiddleOfWord(index) && line.UntilEndOfWordHasChar(index - 1, '_', true)) ||
                       line.IsCloseTag(index);
            }

            return (line.IsOpenTag(index) && (index == 0 || line[index - 1] == ' ' || line[index - 1] == '\\')) ||
                   (line.CharInMiddleOfWord(index) && line.UntilEndOfWordHasChar( index + 1, '_'));
        }
    }
}
