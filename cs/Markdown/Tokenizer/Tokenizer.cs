using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Tokenizer
    {
        private static readonly HashSet<char> EscapableChars = new() { '_', '\\', ' ', '#', '-' };
        private const char EscapeChar = '\\';
        private const char EmphasisChar = '_';
        private const char SpaceChar = ' ';
        private const char NewlineChar = '\n';
        private const char HeaderChar = '#';
        private const char ListChar = '-';

        public List<Token> TextToTokens(string input)
        {
            input = input.Replace("\r\n", "\n").Replace("\r", "\n");
            var tokens = new List<Token>();
            var textBuffer = new StringBuilder();
            int position = 0;

            while (position < input.Length)
            {
                char currentChar = input[position];
                
                switch (currentChar)
                {
                    case EscapeChar:
                        ProcessEscapeSequence(input, ref position, textBuffer);
                        break;
                        
                    case EmphasisChar:
                        ProcessEmphasisToken(input, position, tokens, textBuffer, ref position);
                        break;
                        
                    case SpaceChar:
                        ProcessSpaceToken(input, position, tokens, textBuffer, ref position);
                        break;
                        
                    case NewlineChar:
                        ProcessNewlineToken(position, tokens, textBuffer);
                        position++;
                        break;
                    
                        
                    case HeaderChar:
                        ProcessHeaderToken(input, position, tokens, textBuffer);
                        position++;
                        break;
                        
                    case ListChar:
                        ProcessListToken(input, position, tokens, textBuffer);
                        position++;
                        break;
                        
                    default:
                        textBuffer.Append(currentChar);
                        position++;
                        break;
                }
            }

            FlushTextBuffer(tokens, textBuffer, position);
            return tokens;
        }

        private void ProcessEscapeSequence(string input, ref int position, StringBuilder textBuffer)
        {
            if (position + 1 < input.Length)
            {
                char nextChar = input[position + 1];
        
                if (nextChar == EscapeChar)
                {

                    position += 2;
                }
                else if (EscapableChars.Contains(nextChar))
                {
            
                    textBuffer.Append(nextChar);
                    position += 2;
                }
                else
                {

                    textBuffer.Append(EscapeChar);
                    position++;
                }
            }
            else
            {

                textBuffer.Append(EscapeChar);
                position++;
            }
        }

        private void ProcessEmphasisToken(string input, int currentPosition, List<Token> tokens, 
                                         StringBuilder textBuffer, ref int position)
        {
            FlushTextBuffer(tokens, textBuffer, currentPosition);
            
            int underscoreCount = CountConsecutiveChars(input, position, EmphasisChar);
            var tokenType = GetEmphasisTokenType(underscoreCount);
            
            tokens.Add(new Token(tokenType, new string(EmphasisChar, underscoreCount), currentPosition));
            position += underscoreCount;
        }

        private void ProcessSpaceToken(string input, int currentPosition, List<Token> tokens,
                                      StringBuilder textBuffer, ref int position)
        {
            FlushTextBuffer(tokens, textBuffer, currentPosition);
            
            int spaceStart = position;
            while (position < input.Length && input[position] == SpaceChar)
            {
                position++;
            }
            
            int spaceCount = position - spaceStart;
            tokens.Add(new Token(TokenType.Space, new string(SpaceChar, spaceCount), spaceStart));
        }

        private void ProcessNewlineToken(int currentPosition, List<Token> tokens, StringBuilder textBuffer)
        {
            FlushTextBuffer(tokens, textBuffer, currentPosition);
            tokens.Add(new Token(TokenType.NextLine, "\n", currentPosition));
        }

        private void ProcessHeaderToken(string input, int currentPosition, List<Token> tokens, StringBuilder textBuffer)
        {
            FlushTextBuffer(tokens, textBuffer, currentPosition);
            
            bool isAtStartOfLine = IsAtStartOfLine(input, currentPosition);
            bool hasSpaceAfter = currentPosition + 1 < input.Length && input[currentPosition + 1] == SpaceChar;
            
            var tokenType = (isAtStartOfLine && hasSpaceAfter) ? TokenType.Header : TokenType.Text;
            tokens.Add(new Token(tokenType, "#", currentPosition));
        }

        private void ProcessListToken(string input, int currentPosition, List<Token> tokens, StringBuilder textBuffer)
        {
            FlushTextBuffer(tokens, textBuffer, currentPosition);
            
            bool isAtStartOfLine = IsAtStartOfLine(input, currentPosition);
            bool hasSpaceAfter = currentPosition + 1 < input.Length && input[currentPosition + 1] == SpaceChar;
            
            var tokenType = (isAtStartOfLine && hasSpaceAfter) ? TokenType.ListItem : TokenType.Text;
            tokens.Add(new Token(tokenType, "-", currentPosition));
        }

        private TokenType GetEmphasisTokenType(int underscoreCount)
        {
            switch (underscoreCount)
            {
                case 1: return TokenType.Emphasis;
                case 2: return TokenType.Strong;
                default: return TokenType.Text;
            }
        }

        private bool IsAtStartOfLine(string input, int position)
        {
            return position == 0 || input[position - 1] == NewlineChar;
        }

        private void FlushTextBuffer(List<Token> tokens, StringBuilder textBuffer, int currentPosition)
        {
            if (textBuffer.Length > 0)
            {
                tokens.Add(new Token(TokenType.Text, textBuffer.ToString(), currentPosition - textBuffer.Length));
                textBuffer.Clear();
            }
        }

        private int CountConsecutiveChars(string text, int startPosition, char targetChar)
        {
            int count = 0;
            while (startPosition + count < text.Length && text[startPosition + count] == targetChar)
            {
                count++;
            }
            return count;
        }
    }
}