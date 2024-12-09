using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.TokenParser.TokenHandlers
{
    public abstract class UnderscoreTokensHandler(TagType tagType, string TokenContent)
    {
        private readonly TagType TagType = tagType;

        private readonly string Content = TokenContent;

        public List<Token> HandleLine(List<Token> line)
        {
            var result = new List<Token>();
            var opened = new List<Token>(1);
            var position = 0;
            Func<List<Token>, int, bool> isClosed = (list, i) => false;

            for (var j = 0; j < line.Count; j++)
            {
                if (opened.Count == 0 && IsOpen(j, line))
                {
                    if (IsOpenInWord(j, line))
                    {
                        var token = line[j];
                        isClosed = (list, index) => IsCloseInWord(index, list, token);
                        opened.Add(line[j]);
                    }
                    else if (IsOpenBetweenWords(j, line))
                    {
                        var token = line[j];
                        isClosed = (list, i) => IsClosed(i, list, token);
                        opened.Add(line[j]);
                    }
                }
                else if (opened.Count > 0 && isClosed(line, j))
                {
                    result.Add(opened[0]);
                    result.Add(new Token(TokenType.MdTag, TokenContent,
                        position, true, TagType));

                    opened.Clear();
                }
                else if (line[j].TagType == TagType)
                {
                    result.Add(new Token(TokenType.Text, TokenContent, position));
                }

                position += line[j].Content.Length;
            }

            if (opened.Count > 0)
            {
                opened[0].TagType = TagType.UnDefined;
                opened[0].TokenType = TokenType.Text;

                result.Add(opened[0]);
            }

            return result;
        }

        private bool IsOpen(int index, List<Token> tokens)
        {
            var isFirstInLine = IsFirstInLine(index, tokens);
            var isOpenOrdinary = IsOpenOrdinary(index, tokens);
            var isOpenClosed = IsInWord(index, tokens);

            return isFirstInLine || isOpenOrdinary || isOpenClosed;
        }

        private bool IsClosed(int index, List<Token> tokens, Token token)
        {
            var isLastInLine = IsLastInLine(index, tokens);
            var isClosedOrdinary = IsClosedOrdinary(index, tokens);
            var isOpenClosed = IsCloseInWord(index, tokens, token);

            return isLastInLine || isClosedOrdinary || isOpenClosed;
        }

        private bool IsOpenBetweenWords(int index, List<Token> tokens)
        {
            return IsOpenOrdinary(index, tokens) ^ IsFirstInLine(index, tokens);
        }

        private bool IsOpenInWord(int index, List<Token> tokens)
        {
            return IsInWord(index, tokens);
        }

        private bool IsCloseBetweenWords(int index, List<Token> tokens)
        {
            return IsClosedOrdinary(index, tokens) ^ IsLastInLine(index, tokens);
        }

        private bool IsCloseInWord(int index, List<Token> tokens, Token openToken)
        {
            return index - 2 > -1 && (IsInWord(index, tokens) || IsCloseBetweenWords(index, tokens))
                                  && tokens[index - 2] == openToken;
        }

        #region OpenSituations

        /// <summary>
        ///     Определяет является ли токен одновременно и
        ///     открывающим и закрывающим тегом - случай
        ///     если тэг в слове ("пре_сп_ко_йн_ый)
        /// </summary>
        /// <param name="index">Индекс токена</param>
        /// <param name="tokens">Список токенов для проверки</param>
        /// <returns>true если тег внутри слова иначе false</returns>
        private bool IsInWord(int index, List<Token> tokens)
        {
            return tokens.LastTokenIs(TokenType.Text, index) &&
                   tokens.CurrentTokenIs(TagType, index) &
                   tokens.NextTokenIs(TokenType.Text, index);
        }

        private bool IsFirstInLine(int index, List<Token> tokens)
        {
            return index == 0 && tokens.CurrentTokenIs(TagType, index) &&
                   (tokens.NextTokenIs(TokenType.Text, index) ||
                    tokens.NextTokenIs(TokenType.MdTag, index));
        }

        private bool IsOpenOrdinary(int index, List<Token> tokens)
        {
            var hasWhSpaceBefore = tokens.LastTokenIs(TokenType.WhiteSpace, index);
            var hasMdTagBefore = tokens.LastTokenIs(TokenType.MdTag, index);

            return (hasWhSpaceBefore || hasMdTagBefore) &&
                   tokens.CurrentTokenIs(TagType, index) &&
                   tokens.NextTokenIs(TokenType.Text, index);
        }

        #endregion


        #region CloseSituations

        private bool IsLastInLine(int index, List<Token> tokens)
        {
            var isLast = index == tokens.Count() - 1;

            return isLast && tokens.CurrentTokenIs(TagType, index) &&
                   (tokens.LastTokenIs(TokenType.Text, index) ||
                    tokens.LastTokenIs(TokenType.MdTag, index));
        }

        private bool IsClosedOrdinary(int index, List<Token> tokens)
        {
            var hasTextAfter = tokens.NextTokenIs(TokenType.WhiteSpace, index);
            var hasMdTagAfter = tokens.NextTokenIs(TokenType.MdTag, index);

            return tokens.LastTokenIs(TokenType.Text, index) &&
                   tokens.CurrentTokenIs(TagType, index) &&
                   (hasMdTagAfter || hasTextAfter);
        }

        #endregion
    }
}
