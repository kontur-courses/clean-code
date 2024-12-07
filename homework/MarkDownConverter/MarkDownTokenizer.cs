using MarkDownConverter.Interfaces;
using MarkDownConverter.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter
{
    public class MarkDownTokenizer : ITokenizer
    {
        private int index;
        private readonly List<IToken> tokens = [];
        private const string DoubleLowLine = "__";
        private const string LowLine = "_";
        private const string Slash = "\\";
        private const char LowLineChar = '_';
        private const char HeadChar = '#';
        private const char SlashChar = '\\';
        private const char NewLineChar = '\n';
        private const char SpaceChar = ' ';
        private readonly char[] serviceChars = [HeadChar, LowLineChar, SlashChar, NewLineChar];

        public List<IToken> Tokenize(string text)
        {
            index = 0;
            var validatingStach = new Stack<string>();

            while (index < text.Length)
            {
                switch (text[index])
                {
                    case SpaceChar:
                        TokenizeSpace();
                        break;
                    case NewLineChar:
                        TokenizeNewLine(validatingStach);
                        break;
                    case SlashChar:
                        TokenizeSlash(text);
                        break;
                    case LowLineChar:
                        TokenizeItalicOrBold(text, validatingStach);
                        break;
                    case HeadChar:
                        TokenizeHeading(text);
                        break;
                    default:
                        TokenizeSymbols(text);
                        break;
                }
            }

            return tokens;
        }

        private void TokenizeSpace() => tokens.Add(new SpaceToken(index++));

        private void TokenizeHeading(string text)
        {
            if (NextSpace(text) && IsParagraphBeginning(text)) 
                tokens.Add(new HeadingToken(index++));

            else tokens.Add(new SymbolsToken(index, "#"));
            index++;
        }

        private void TokenizeSymbols(string text)
        {
            var value = new StringBuilder();
            var start = index;
            var serviceChars = new char[] { HeadChar, LowLineChar, NewLineChar, SlashChar, SpaceChar };
            while (index < text.Length && !serviceChars.Contains(text[index]) && !IsDigit(text))
                value.Append(text[index++]);

            if (value.Length > 0) tokens.Add(new SymbolsToken(start, value.ToString()));
            if (index < text.Length && IsDigit(text)) TokenizeNumber(text);
        }


        private void TokenizeNumber(string text)
        {
            var sb = new StringBuilder();
            var start = index;
            while (index < text.Length && (IsDigit(text) || text[index] == LowLineChar))
                sb.Append(text[index++]);
            tokens.Add(new NumbersToken(start, sb.ToString()));
        }

        private void TokenizeItalicOrBold(string text, Stack<string> stack)
        {
            var isDoubleLowLine = NextLowLine(text);
            var isTripleLowLine = NextDoubleLowLine(text);
            var isSingleLowLine = !isTripleLowLine && !isDoubleLowLine;
            if (stack.Count == 0) TokenizeItalicOrBoldWthStackEmpty(isSingleLowLine, isTripleLowLine, stack);
            else if (stack.Count == 1)
                TokenizeItalicOrBoldWithStackHasOne(isSingleLowLine, isDoubleLowLine, isTripleLowLine, stack);
            else if (stack.Count == 2) TokenizeItalicOrBoldWithStackHasTwo(isSingleLowLine, isTripleLowLine, stack);
        }

        private void TokenizeItalicOrBoldWthStackEmpty(
            bool isSingleLowLine, 
            bool isTripleLowLine,
            Stack<string> stack)
        {
            if (isSingleLowLine)
            {
                TokenizeItalic();
                stack.Push(LowLine);
                return;
            }

            TokenizeBold();
            stack.Push(DoubleLowLine);
            if (!isTripleLowLine) return;
            TokenizeItalic();
            stack.Push(LowLine);
        }

        private void TokenizeItalicOrBoldWithStackHasOne(
            bool isSingleLowLine,
            bool isDoubleLowLine,
            bool isTripleLowLine,
            Stack<string> stack)
        {
            switch (stack.Peek())
            {
                case DoubleLowLine when isSingleLowLine:
                    TokenizeItalic();
                    stack.Push(LowLine);
                    break;
                case DoubleLowLine:
                    {
                        if (isTripleLowLine) TokenizeItalic();
                        TokenizeBold();
                        stack.Pop();
                        break;
                    }
                case LowLine:
                    {
                        if (isTripleLowLine)
                        {
                            TokenizeBold();
                            TokenizeItalic();
                        }
                        else if (isDoubleLowLine)
                        {
                            tokens.Add(new SymbolsToken(index, DoubleLowLine));
                            index += 2;
                        }
                        else TokenizeItalic();

                        stack.Pop();
                        break;
                    }
            }
        }

        private void TokenizeItalicOrBoldWithStackHasTwo(
            bool isSingleLowLine, 
            bool isTripleLowLine,
            Stack<string> stack)
        {
            if (isSingleLowLine)
            {
                TokenizeItalic();
                stack.Pop();
                return;
            }

            if (isTripleLowLine) TokenizeItalic();
            TokenizeBold();

            stack.Pop();
            stack.Pop();
        }

        private void TokenizeBold()
        {
            tokens.Add(new BoldToken(index));
            index += 2;
        }

        private void TokenizeItalic()
        {
            tokens.Add(new ItalicToken(index));
            index++;
        }

        private void TokenizeNewLine(Stack<string> stack)
        {
            tokens.Add(new NewLineToken(index));
            stack.Clear();
            index++;
        }

        private void TokenizeSlash(string text)
        {
            if (index + 1 >= text.Length)
            {
                tokens.Add(new SymbolsToken(index++, Slash));
                return;
            }

            if (NextDoubleLowLine(text))
            {
                tokens.Add(new SymbolsToken(index, DoubleLowLine));
                index += 3;
                return;
            }

            var next = text[index + 1];
            tokens.Add(serviceChars.Contains(next)
                ? new SymbolsToken(index, next.ToString())
                : new SymbolsToken(index, Slash + next));
            index += 2;
        }

        private bool NextDoubleLowLine(string text) =>
            index + 2 < text.Length && text[index + 1] == LowLineChar && text[index + 2] == LowLineChar;

        private bool NextSpace(string text) => index + 1 < text.Length && text[index + 1] == SpaceChar;
        private bool NextLowLine(string text) => index + 1 < text.Length && text[index + 1] == LowLineChar;
        private bool IsDigit(string text) => char.IsDigit(text[index]);

        private bool IsParagraphBeginning(string text) =>
            index == 0 
            || (index > 0 && text[index - 1] == NewLineChar);
    }
}
