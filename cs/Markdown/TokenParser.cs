using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenParser
    {
        private Stack<IToken> openedTokens;
        private StringBuilder line;


        public TokenParser()
        {
            openedTokens = new Stack<IToken>();
        }

        public void Parse(IToken token, StringBuilder inputLine)
        {
            line = inputLine;
            openedTokens.Push(token);
            for (var i = 0; i < line.Length; i++)
            {
                if (i == openedTokens.Peek().EndPosition - openedTokens.Peek().ClosingTag.MdTag.Length)
                {
                    i += openedTokens.Peek().ClosingTag.MdTag.Length;
                    openedTokens.Pop();
                    if (i == line.Length)
                        break;
                }
                if (TryPassEscapeChar(i))
                    i++;
                Tag tag;
                if (!TryReadTag(ref i, true, out tag) || CheckConflicts(tag))
                    continue;
                if (tag.IsOpening)
                    TryCloseTag(i, tag);
            }
        }

        private bool TryReadTag(ref int position, bool isOpening, out Tag foundTag)
        {
            foreach (var tag in Tag.AllTags.Where(t => t.IsOpening == isOpening && t.TokenType != TokenType.Simple))
            {
                if ((tag.MdTag.Length == 1 && line[position] == tag.MdTag[0])
                    || (position != line.Length - 1
                        && line[position] == tag.MdTag[0] && line[position + 1] == tag.MdTag[1]))
                {
                    if (tag.TokenType == TokenType.Header
                        || !tag.IsOpening && !CheckSpaceBeforeTag(position)
                        || tag.IsOpening && !CheckSpaceAfterTag(position, tag.MdTag.Length))
                    {
                        foundTag = tag;
                        position += tag.MdTag.Length - 1;
                        return true;
                    }
                    position += tag.MdTag.Length - 1;
                    break;
                }
            }
            foundTag = null;
            return false;
        }

        private bool CheckConflicts(Tag tag)
        {
            return (tag.TokenType == TokenType.Strong && openedTokens.Peek().Type == TokenType.Italic)
                || (tag.TokenType == TokenType.Header && openedTokens.Peek().Type != TokenType.Simple);
        }

        private bool TryCloseTag(int startPosition, Tag openingTag)
        {
            var containsOnlyDigits = true;
            for (var i = startPosition + 1; i < line.Length; i++)
            {
                if (TryPassEscapeChar(i))
                    i++;
                if (IsNewLine(i, openingTag) || IsEndOfParentToken(i) || IsSpaceAfterPartOfWord(i, startPosition, openingTag))
                    return false;
                if (IsValidTag(ref i, startPosition, openingTag, containsOnlyDigits))
                {
                    OpenToken(openingTag, startPosition);
                    CloseToken(i);
                    return true;
                }
                if (containsOnlyDigits && !char.IsDigit(line[i]))
                    containsOnlyDigits = false;
            }

            if (openingTag.TokenType == TokenType.Header)
            {
                OpenToken(openingTag, startPosition);
                CloseToken(line.Length - 1);
                return true;
            }
            return false;
        }

        private bool IsNewLine(int i, Tag openingTag)
        {
            return line[i] == '\n' && openingTag.TokenType != TokenType.Header;
        }

        private bool IsEndOfParentToken(int i)
        {
            return i == openedTokens.Peek().EndPosition - openedTokens.Peek().ClosingTag.MdTag.Length
                    && openedTokens.Peek().Type != TokenType.Header;
        }

        private bool IsSpaceAfterPartOfWord(int i, int startPosition, Tag openingTag)
        {
            return line[i] == ' ' && startPosition - openingTag.MdTag.Length >= 0
                                  && char.IsLetterOrDigit(line[startPosition - openingTag.MdTag.Length]);
        }

        private bool IsValidTag(ref int i, int startPosition, Tag openingTag, bool containsOnlyDigits)
        {
            Tag metTag;
            return TryReadTag(ref i, false, out metTag) && !metTag.IsOpening && metTag.TokenType == openingTag.TokenType
                   && !CheckEmptyTag(startPosition, i, openingTag, metTag) && !containsOnlyDigits;
        }

        private bool CheckEmptyTag(int startPos, int closePos, Tag openingTag, Tag closingTag)
        {
            return (closePos - startPos + 1) <= (openingTag.MdTag.Length + closingTag.MdTag.Length);
        }

        private void CloseToken(int i)
        {
            openedTokens.Peek().Close(i);
        }

        private void OpenToken(Tag tag, int i)
        {
            new Token(i - tag.MdTag.Length + 1, 0, openedTokens.Peek(), tag.TokenType).Open(openedTokens);
        }

        private bool CheckSpaceAfterTag(int position, int tagLength)
        {
            return position != line.Length - tagLength && line[position + tagLength] == ' ';
        }

        private bool CheckSpaceBeforeTag(int position)
        {
            return position != 0 && line[position - 1] == ' ';
        }

        private bool TryPassEscapeChar(int position)
        {
            if (line[position] == '\\' && position != line.Length - 1)
            {
                var temporaryPosition = position + 1;
                if (TryReadTag(ref temporaryPosition, true, out _) || TryReadTag(ref temporaryPosition, false, out _)
                                                                   || line[position + 1] == '\\')
                {
                    line.Remove(position, 1);
                    openedTokens.Peek().Length--;
                    return true;
                }
            }

            return false;
        }
    }
}
