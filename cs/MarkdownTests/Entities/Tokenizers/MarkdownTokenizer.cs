using System.Runtime.CompilerServices;
using System.Text;
using Markdown.Enums;
using Markdown.Models;
using Newtonsoft.Json.Linq;

namespace Markdown.Entities.Parsers
{
    /// <summary>
    /// Tokenizer который работает с markdown текстом
    /// </summary>
    public class MarkdownTokenizer : ITokenizer
    {
        private const char Underscore = '_';
        private const string DoubleUnderscore = "__";
        private const char EscapeCharacter = '\\';
        private const char SpaceCharacter = ' ';
        private const string HeaderCharacter = "# ";
        private const char SquareOpen = '[';
        private const char SquareClose = ']';
        private const char RoundOpen = '(';
        private const char RoundClose = ')';
        public List<Token> Tokenize(string text)
        {
            return TokenizeLines(TextToLines(text));
        }

        public List<string> TextToLines(string text)
        {
            return text.Split("\n").ToList();
        }

        public List<Token> TokenizeLines(IEnumerable<string> lines)
        {
            var tokens = new List<Token>();
            foreach (var line in lines)
            {
                tokens.AddRange(TokenizeLine(line));
                tokens.Add(new Token(TokenType.Newline));
            }
            return tokens;
        }

        /// <summary>
        /// Ключевые правила из спецификации:
        /// Приоритет обработки: экранирование → заголовки → жирный → курсив
        /// Вложенность: жирный может содержать курсив, но не наоборот
        /// Экранирование: \ отменяет разметку следующего символа
        /// Заголовки: только в начале строки с #
        /// Валидация: проверки на пробелы, цифры, пересечение слов
        /// Непарные теги: остаются как обычный текст
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<Token> TokenizeLine(string line)
        {
            var tokens = new List<Token>();
            var currentPosition = 0;
            var textBuffer = new StringBuilder();

            if (IsHeader(line))
            {
                ProcessHeader(line, ref currentPosition, tokens);
            }

            while (currentPosition < line.Length)
            {
                var currentChar = line[currentPosition];

                if (currentChar == EscapeCharacter)
                {
                    ProcessEscapeCharacter(line, ref currentPosition, textBuffer);
                    continue;
                }

                if (IsBoldMarker(line, currentPosition))
                {
                    ProcessBoldMarker(line, ref currentPosition, tokens, textBuffer, TokenType.Text);
                    continue;
                }

                if (IsItalicsMarker(line, currentPosition))
                {
                    ProcessItalicsMarker(line, ref currentPosition, tokens, textBuffer, false, TokenType.Text);
                    continue;
                }

                if (currentChar == SquareOpen)
                {
                    ProcessLink(line, ref currentPosition, tokens, textBuffer);
                    continue;
                }

                textBuffer.Append(currentChar);
                currentPosition++;
            }

            if(textBuffer.Length > 0) FlushTextBufferIfNotEmpty(textBuffer, tokens, TokenType.Text);
            return tokens;
        }

        private void ProcessLink(string line, ref int currentPosition, List<Token> tokens, StringBuilder textBuffer)
        {
            FlushTextBufferIfNotEmpty(textBuffer, tokens, TokenType.Text);

            tokens.Add(new Token(TokenType.LinkStart));
            currentPosition++;

            var linkTextBuffer = new StringBuilder();

            while (currentPosition < line.Length && line[currentPosition] != SquareClose)
            {
                if (line[currentPosition] == EscapeCharacter)
                {
                    ProcessEscapeCharacter(line, ref currentPosition, linkTextBuffer);
                }

                if (IsBoldMarker(line, currentPosition))
                {
                    ProcessBoldMarker(line, ref currentPosition, tokens, linkTextBuffer, TokenType.LinkText);
                    continue;
                }

                if (IsItalicsMarker(line, currentPosition))
                {
                    ProcessItalicsMarker(line, ref currentPosition, tokens, linkTextBuffer, false, TokenType.LinkText);
                    continue;
                }

                linkTextBuffer.Append(line[currentPosition]);
                currentPosition++;

            }

            if (linkTextBuffer.Length > 0)
            {
                tokens.Add(new Token(TokenType.LinkText, linkTextBuffer.ToString()));
            }

            if (currentPosition < line.Length && line[currentPosition] == SquareClose)
            {
                tokens.Add(new Token(TokenType.LinkEnd));
                currentPosition++;
            }

            if (currentPosition < line.Length && line[currentPosition] == RoundOpen)
            {
                ProcessUrl(line, ref currentPosition, tokens);
            }
        }

        private void ProcessUrl(string line, ref int currentPosition, List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.UrlStart));
            currentPosition++;

            var urlBuffer = new StringBuilder();
            var titleBuffer = new StringBuilder();
            var isParsingTitle = false;

            while (currentPosition < line.Length && line[currentPosition] != RoundClose)
            {
                if (line[currentPosition] == EscapeCharacter)
                {
                    ProcessEscapeCharacter(line, ref currentPosition, isParsingTitle ? titleBuffer : urlBuffer);
                }
                else if (line[currentPosition] == '"' && !isParsingTitle && urlBuffer.Length > 0)
                {
                    isParsingTitle = true;
                    tokens.Add(new Token(TokenType.Url, urlBuffer.ToString()));
                    tokens.Add(new Token(TokenType.UrlTitleDelimiter));
                    currentPosition++;
                }
                else
                {
                    if (isParsingTitle)
                    {
                        titleBuffer.Append(line[currentPosition]);
                    }
                    else
                    {
                        urlBuffer.Append(line[currentPosition]);
                    }
                    currentPosition++;
                }
            }

            if (!isParsingTitle && urlBuffer.Length > 0)
            {
                tokens.Add(new Token(TokenType.Url, urlBuffer.ToString()));
            }
            else if (isParsingTitle && titleBuffer.Length > 0)
            {
                tokens.Add(new Token(TokenType.UrlTitle, titleBuffer.ToString()));
                tokens.Add(new Token(TokenType.UrlTitleDelimiter));
            }

            if (currentPosition < line.Length && line[currentPosition] == RoundClose)
            {
                tokens.Add(new Token(TokenType.UrlEnd));
                currentPosition++;
            }
        }

        /// <summary>
        /// Метод обработки тэга курсива, превращает часть строки в набор токенов если она удовлетворяет условиям спецификации
        /// Дополнительно обрабатывает случай неправильной вложенности тэга курсива и полужирного тэга
        /// </summary>
        /// <param name="line"></param>
        /// <param name="currentPosition"></param>
        /// <param name="tokens"></param>
        /// <param name="originalTextBuffer"></param>
        /// <param name="isNestedCall"> - флаг для обработки случая когда находим курсив внутри полужирного текста</param>
        private void ProcessItalicsMarker(string line, ref int currentPosition, List<Token> tokens, StringBuilder originalTextBuffer, 
            bool isNestedCall, TokenType textType)
        {
            var isInWord = false;
            var startPosition = currentPosition;
            var foundBoldMarker = false;

            var textBuffer = new StringBuilder();

            if (IsMarkerInsideWord(line, currentPosition, false)) isInWord = true;

            SkipItalicsMarker(ref currentPosition);

            while (currentPosition < line.Length)
            {
                var currentChar = line[currentPosition];

                if (IsValidClosingMarker(line, currentPosition, false))
                {
                    var isEmptyWord = IsEmptyMarkedWord(currentPosition, startPosition, false);
                    if (isEmptyWord)
                    {
                        originalTextBuffer.Append(textBuffer);
                        return;
                    }

                    if (foundBoldMarker)
                    {
                        textBuffer.Insert(0, Underscore);
                        originalTextBuffer.Append(textBuffer);
                        return;
                    }

                    FlushTextBufferIfNotEmpty(originalTextBuffer, tokens, textType);
                    AddTokensFromBufferWithSpecifiedTags(textBuffer, tokens, TokenType.ItalicsStart, TokenType.ItalicsEnd);
                    SkipItalicsMarker(ref currentPosition);
                    return;
                }

                if (currentChar == Underscore)
                {
                    if (line[currentPosition + 1] == Underscore)
                    {
                        if (isNestedCall)
                        {
                            textBuffer.Append(DoubleUnderscore);
                            SkipBoldMarker(ref currentPosition);

                            originalTextBuffer.Append(Underscore);
                            originalTextBuffer.Append(textBuffer);
                            return;
                        }
                        //Меняем состояние флага на обратное
                        foundBoldMarker = !foundBoldMarker;
                    }
                }

                if (currentChar == SpaceCharacter)
                {
                    if (isInWord)
                    {
                        originalTextBuffer.Append(Underscore);
                        originalTextBuffer.Append(textBuffer);
                        return;
                    }

                }

                if (currentChar == EscapeCharacter)
                {
                    ProcessEscapeCharacter(line, ref currentPosition, textBuffer);
                    continue;
                }

                textBuffer.Append(currentChar);
                currentPosition++;
            }

            textBuffer.Insert(0, Underscore);
            originalTextBuffer.Append(textBuffer);
        }

        /// <summary>
        /// Метод обработки тэга полужирного текста, превращает часть строки в набор токенов если она удовлетворяет условиям спецификации
        /// </summary>
        /// <param name="line"></param>
        /// <param name="currentPosition"></param>
        /// <param name="tokens"></param>
        /// <param name="originalTextBuffer"></param>
        private void ProcessBoldMarker(string line, ref int currentPosition, List<Token> tokens, 
            StringBuilder originalTextBuffer, TokenType textType)
        {
            var isClosingFound = false;
            var isInWord = IsMarkerInsideWord(line, currentPosition, true);
            var startPosition = currentPosition;

            var textBuffer = new StringBuilder();
            var innerTokens = new List<Token>();

            SkipBoldMarker(ref currentPosition);

            while (currentPosition < line.Length)
            {
                var currentChar = line[currentPosition];

                if (IsValidClosingMarker(line, currentPosition, true))
                {
                    var isEmptyWord = IsEmptyMarkedWord(currentPosition, startPosition, true);
                    if (isEmptyWord)
                    {
                        textBuffer.Append(DoubleUnderscore);
                        originalTextBuffer.Append(textBuffer);
                        return;
                    }

                    bool shouldProcessNestedItalics = innerTokens.Count > 0;

                    if (shouldProcessNestedItalics)
                    {
                        ProcessNestedItalicsTokens(originalTextBuffer, textBuffer, tokens, innerTokens,
                            ref currentPosition, TokenType.Text);
                    }
                    else
                    {
                        ProcessBoldTokens(originalTextBuffer, textBuffer, tokens, ref currentPosition, textType);
                    }

                    return;
                }

                if (currentChar == Underscore)
                {
                    ProcessItalicsMarker(line, ref currentPosition, innerTokens, textBuffer, true, TokenType.Text);
                    continue;
                }

                if (currentChar == SpaceCharacter)
                {
                    if (isInWord)
                    {
                        originalTextBuffer.Append(DoubleUnderscore);
                        originalTextBuffer.Append(textBuffer);
                        return;
                    }

                }

                if (currentChar == EscapeCharacter)
                {
                    ProcessEscapeCharacter(line, ref currentPosition, textBuffer);
                    continue;
                }

                textBuffer.Append(currentChar);
                currentPosition++;
            }

            textBuffer.Insert(0, DoubleUnderscore);
            originalTextBuffer.Append(textBuffer);
        }

        private void ProcessBoldTokens(StringBuilder originalTextBuffer, StringBuilder textBuffer, List<Token> tokens, 
            ref int currentPosition, TokenType textType)
        {
            FlushTextBufferIfNotEmpty(originalTextBuffer, tokens, textType);
            AddTokensFromBufferWithSpecifiedTags(textBuffer, tokens, TokenType.BoldStart, TokenType.BoldEnd);
            SkipBoldMarker(ref currentPosition);
        }

        private void ProcessNestedItalicsTokens(StringBuilder originalTextBuffer, StringBuilder textBuffer, 
            List<Token> tokens, List<Token> innerTokens, ref int currentPosition, TokenType textType)
        {
            FlushTextBufferIfNotEmpty(originalTextBuffer, tokens, textType);
            AddNestedItalicsTokens(textBuffer, tokens, innerTokens);
            SkipBoldMarker(ref currentPosition);
        }

        private void AddNestedItalicsTokens(StringBuilder textBuffer, List<Token> tokens, List<Token> innerTokens)
        {
            tokens.Add(new Token(TokenType.BoldStart));
            tokens.AddRange(innerTokens);
            FlushTextBufferIfNotEmpty(textBuffer, tokens, TokenType.Text);
            tokens.Add(new Token(TokenType.BoldEnd));
        }

        private void AddTokensFromBufferWithSpecifiedTags(StringBuilder textBuffer, List<Token> tokens, TokenType startToken, TokenType endToken)
        {
            tokens.Add(new Token(startToken));
            FlushTextBufferIfNotEmpty(textBuffer, tokens, TokenType.Text);
            tokens.Add(new Token(endToken));
        }

        private bool IsEmptyMarkedWord(int currentPosition, int startPosition, bool isBold)
        {
            if (isBold) return currentPosition - startPosition == 2;
            return currentPosition - startPosition == 1;
        }

        private void SkipItalicsMarker(ref int currentPosition)
        {
            currentPosition++;
        }

        private void SkipBoldMarker(ref int currentPosition)
        {
            currentPosition += 2;
        }

        //проверяем что открывающий маркер находится внутри слова
        private bool IsMarkerInsideWord(string line, int currentPosition, bool isBold)
        {
            if (currentPosition == 0) return false;

            if (isBold)
            {
                return currentPosition + 2 < line.Length
                    && char.IsLetter(line[currentPosition - 1]) 
                    && char.IsLetter(line[currentPosition + 2]);
            }

            return (currentPosition + 1 < line.Length) 
                   && char.IsLetter(line[currentPosition - 1])
                   && char.IsLetter(line[currentPosition + 1]);
        }

        private bool IsValidClosingMarker(string line, int currentPosition, bool isBold)
        {
            if (isBold)
            {
                return currentPosition + 1 < line.Length
                    && line[currentPosition] == Underscore
                    && line[currentPosition + 1] == Underscore
                    && !char.IsWhiteSpace(line[currentPosition - 1]);
            }
            //обработка для курсива
            if (currentPosition + 1 < line.Length)
            {
                return line[currentPosition] == Underscore
                       && line[currentPosition + 1] != Underscore
                       && line[currentPosition - 1] != Underscore
                       && !char.IsWhiteSpace(line[currentPosition - 1]);
            }
            return line[currentPosition] == Underscore
                   && !char.IsWhiteSpace(line[currentPosition - 1]);
        }

        private bool IsItalicsMarker(string line, int currentPosition)
        {
            return currentPosition + 1 < line.Length
                && line[currentPosition] == Underscore && line[currentPosition + 1] != Underscore
                && !IsSpaceAfterMarker(line, currentPosition, false)
                && !IsAmongDigits(line, currentPosition, false);
        }

        private bool IsAmongDigits(string line, int currentPosition, bool isBold)
        {
            //если маркер не в самом начале то посмотрим назад
            if (currentPosition > 0)
            {
                if (isBold)
                {
                    return currentPosition + 2 < line.Length
                        && (char.IsDigit(line[currentPosition - 1]) || char.IsDigit(line[currentPosition + 2]));
                }
                return char.IsDigit(line[currentPosition - 1]) 
                       || char.IsDigit(line[currentPosition + 1]);
            }

            //если маркер в начале строки посмотрим что числа только спереди
            if (isBold)
            {
                return currentPosition + 2 < line.Length
                    && char.IsDigit(line[currentPosition + 2]);
            }
            return char.IsDigit(line[currentPosition + 1]);
        }

        //проверим что текущий и след.символы у нас подчеркивания и непробельный символ после
        private bool IsBoldMarker(string line, int currentPosition)
        {
            return currentPosition + 1 < line.Length
                    && line[currentPosition] == Underscore 
                    && line[currentPosition + 1] == Underscore
                    && !IsSpaceAfterMarker(line, currentPosition, true)
                    && !IsAmongDigits(line, currentPosition, true);
        }

        private bool IsSpaceAfterMarker(string line, int currentPosition, bool isBold)
        {
            if(isBold) return char.IsWhiteSpace(line[currentPosition + 2]);
            return char.IsWhiteSpace(line[currentPosition + 1]);
        }

        private void FlushTextBufferIfNotEmpty(StringBuilder textBuffer, List<Token> tokens, TokenType textType)
        {
            if (textBuffer.Length > 0)
            {
                tokens.Add(new Token(textType, textBuffer.ToString()));
                textBuffer.Clear();
            }
        }

        private void ProcessEscapeCharacter(string line, ref int currentPosition, StringBuilder textBuffer)
        {
            var anyTagsAfter = IsEscapeBeforeMarkers(line, currentPosition);
            if (anyTagsAfter)
            {
                //пропустить текущий, добавить новый в буфер, перейти вперед снова
                if (currentPosition + 2 < line.Length)
                {
                    currentPosition++;
                    textBuffer.Append(line[currentPosition]);
                    if (line[currentPosition] == Underscore && line[currentPosition + 1] == Underscore)
                    {
                        currentPosition++;
                        textBuffer.Append(line[currentPosition]);
                    }
                    if (currentPosition + 1 < line.Length)
                    {
                        currentPosition++;
                    }
                }
                else if (currentPosition + 1 < line.Length)
                {
                    currentPosition++;
                    textBuffer.Append(line[currentPosition]);
                    currentPosition++;
                }
            }
            else
            {
                //записать текущий слэш как есть
                if (currentPosition < line.Length)
                {
                    textBuffer.Append(line[currentPosition]);
                    currentPosition++;
                }
            }
        }

        //Находится ли символ экранирования перед маркерами или другим экранированием
        private bool IsEscapeBeforeMarkers(string line, int currentPosition)
        {
            if (currentPosition + 2 < line.Length)
            {
                return (line[currentPosition + 1] == Underscore ||
                        line[currentPosition + 1] == EscapeCharacter ||
                        (line[currentPosition + 1] == Underscore && line[currentPosition + 2] == Underscore));
            }
            else if (currentPosition + 1 < line.Length)
            {
                return (line[currentPosition + 1] == Underscore ||
                        line[currentPosition + 1] == EscapeCharacter);
            }
            return false;
        }

        private void ProcessHeader(string line, ref int currentPosition, List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.Header));
            if (line.Length > 2) currentPosition = 2;
        }

        private bool IsHeader(string line)
        {
            return line.StartsWith(HeaderCharacter);
        }

    }
}
