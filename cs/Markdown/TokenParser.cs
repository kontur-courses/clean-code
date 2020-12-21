using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenParser
    {
        private Stack<Token> openedTokens;
        private StringBuilder line;


        public TokenParser()
        {
            openedTokens = new Stack<Token>();
        }

        public void Parse(Token token, StringBuilder inputLine)
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
                if (TryReadTag(i, true, out tag))
                {
                    if (!CheckSpacesNextToTag(tag, i) && !CheckConflicts(tag))
                        TryCloseTag(i, tag);
                    i += tag.MdTag.Length - 1;
                }
            }
        }

        private bool TryReadTag(int position, bool isOpening, out Tag foundTag)
        {
            foreach (var tag in Tag.AllTags.Where(t => t.IsOpening == isOpening && t.TokenType != TokenType.Simple))
            {
                if ((tag.MdTag.Length == 1 && line[position] == tag.MdTag[0])
                    || (position != line.Length - 1 && line[position] == tag.MdTag[0] 
                                                    && line[position + 1] == tag.MdTag[1]))
                {
                    foundTag = tag;
                    return true;
                }
            }
            foundTag = null;
            return false;
        }

        private bool CheckSpacesNextToTag(Tag tag, int position)
        {
            if (tag.TokenType != TokenType.Header
                && (!tag.IsOpening && CheckSpaceBeforeTag(position) 
                    || tag.IsOpening && CheckSpaceAfterTag(position, tag.MdTag.Length)))
                return true;
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
            for (var i = startPosition + openingTag.MdTag.Length; i < line.Length; i++)
            {
                if (TryPassEscapeChar(i))
                    i++;
                if (IsNewLine(i, openingTag) || IsEndOfParentToken(i) || IsSpaceAfterPartOfWord(i, startPosition, openingTag))
                    return false;
                Tag closingTag;
                if (TryReadTag(i, false, out closingTag))
                {
                    if (IsValidTag(i, startPosition, openingTag, closingTag, containsOnlyDigits))
                    {
                        OpenToken(openingTag, startPosition);
                        CloseToken(i);
                        return true;
                    }
                    i+= closingTag.MdTag.Length - 1;
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

        private bool IsValidTag(int i, int startPosition, Tag openingTag, Tag closingTag, bool containsOnlyDigits)
        {
            return !CheckSpacesNextToTag(closingTag, i) && !closingTag.IsOpening 
                   && closingTag.TokenType == openingTag.TokenType 
                   && !IsEmptyTag(startPosition, i, openingTag) && !containsOnlyDigits;
        }

        private bool IsEmptyTag(int startPos, int closePos, Tag openingTag)
        {
            return (closePos - startPos - openingTag.MdTag.Length) == 0;
        }

        private void CloseToken(int i)
        {
            openedTokens.Peek().Close(i);
        }

        private void OpenToken(Tag tag, int i)
        {
            new Token(i, 0, openedTokens.Peek(), tag.TokenType).Open(openedTokens);
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
                if (TryReadTag(temporaryPosition, true, out _) || TryReadTag(temporaryPosition, false, out _)
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
