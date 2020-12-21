using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenParser
    {
        private Stack<Token> openedTokens;
        private string line;


        public TokenParser(string inputLine)
        {
            openedTokens = new Stack<Token>();
            line = inputLine;
        }

        public void Parse(Token token)
        {
            openedTokens.Push(token);
            for (var i = 0; i < line.Length; i++)
            {
                if (IsEndOfParentToken(i))
                {
                    MoveIndexerThroughTag(ref i, openedTokens.Peek().ClosingTag);
                    openedTokens.Pop();
                    continue;
                }
                if (TryEscapeTag(i, out var escapedTag))
                {
                    i++;
                    MoveIndexerThroughTag(ref i, escapedTag);
                    continue;
                }
                if (TryReadTag(i, true, out var tag))
                {
                    if (!CheckSpacesNextToTag(tag, i) && !TagConflictsWithParentToken(tag, openedTokens.Peek()))
                        TryCloseTag(i, tag);
                    MoveIndexerThroughTag(ref i, tag);
                }
            }
        }

        private void MoveIndexerThroughTag(ref int indexer, Tag tag)
        {
            indexer += tag.MdTag.Length - 1;
        }

        private bool TryReadTag(int position, bool isOpening, out Tag foundTag)
        {
            foreach (var tag in Tag.AllTags.Where(t => t.IsOpening == isOpening && t.TokenType != TokenType.Simple))
            {
                if (IsTagOnPosition(tag, position))
                {
                    foundTag = tag;
                    return true;
                }
            }
            foundTag = null;
            return false;
        }

        private bool IsTagOnPosition(Tag tag, int position)
        {
            return (tag.MdTag.Length == 1 && line[position] == tag.MdTag[0])
                   || (position != line.Length - 1 && line[position] == tag.MdTag[0] 
                   && line[position + 1] == tag.MdTag[1]);
        }

        private bool CheckSpacesNextToTag(Tag tag, int position)
        {
            if (tag.TokenType != TokenType.Header
                && (!tag.IsOpening && CheckSpaceBeforeTag(position) 
                    || tag.IsOpening && CheckSpaceAfterTag(position, tag.MdTag.Length)))
                return true;
            return false;
        }

        private bool TagConflictsWithParentToken(Tag tag, Token parentToken)
        {
            return (tag.TokenType == TokenType.Strong && parentToken.Type == TokenType.Italic)
                || (tag.TokenType == TokenType.Header && parentToken.Type != TokenType.Simple);
        }

        private bool TryCloseTag(int startPosition, Tag openingTag)
        {
            var containsOnlyDigits = true;
            for (var i = startPosition + openingTag.MdTag.Length; i < line.Length; i++)
            {
                if (TryEscapeTag(i, out var escapedTag))
                {
                    i++;
                    MoveIndexerThroughTag(ref i, escapedTag);
                    continue;
                }
                if (IsNewLine(i, openingTag) || IsEndOfParentToken(i) || IsSpaceAfterPartOfWord(i, startPosition, openingTag))
                    return false;
                if (TryReadTag(i, false, out var closingTag))
                {
                    if (IsValidTag(i, startPosition, openingTag, closingTag, containsOnlyDigits))
                    {
                        OpenToken(openingTag, startPosition);
                        CloseToken(i);
                        return true;
                    }
                    MoveIndexerThroughTag(ref i, closingTag);
                }
                else if (containsOnlyDigits && !char.IsDigit(line[i]))
                    containsOnlyDigits = false;
            }

            return TryCloseHeaderTag(startPosition, openingTag);
        }

        private bool TryCloseHeaderTag(int startPosition, Tag openingTag)
        {
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

        private bool TryEscapeTag(int position, out Tag escapedTag)
        {
            escapedTag = null;
            if (line[position] == '\\' && position != line.Length - 1 
               && (TryReadTag(position + 1, true, out escapedTag) || TryReadTag(position + 1, false, out escapedTag) 
               || line[position + 1] == '\\'))
            {
                openedTokens.Peek().EscapedCharsPos.Add(position);
                return true;
            }

            return false;
        }
    }
}
