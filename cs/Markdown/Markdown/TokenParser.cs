using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class TokenParser
    {
        public readonly IEnumerable<Mark> Marks;

        public TokenParser(IEnumerable<Mark> marks)
        {
            Marks = marks;
        }

        public IEnumerable<Token> Parse(string text)
        {
            var stack = new Stack<Token>();
            var backSlashesCount = 0;
            var index = 0;
            while (index<text.Length)
            {
                if (text[index] == '\\')
                    backSlashesCount++;
                var isBackSlashed = backSlashesCount % 2 == 1;
                if (text[index] != '\\')
                    backSlashesCount = 0;
                var mark = DefineMarkAt(text, index, isBackSlashed);
                if (mark!=null)
                {
                    foreach (var token in GetFirstRawToken(stack, index))
                        yield return token;
                    if (stack.Count != 0)
                        foreach (var token in GetClosedTokensAndAddNotClosed(stack, mark, index, text))
                            yield return token;
                    else
                        stack.Push(new Token(index + mark.Sign.Length, mark));
                    index += mark.Sign.Length - 1;
                }
                if (stack.Count == 0 && mark == null)
                    stack.Push(new Token(index, Mark.RawMark));
                index++;
            }
            foreach (var token in getLastRawToken(stack, index))
                yield return token;
        }

        private IEnumerable<Token> GetClosedTokensAndAddNotClosed(Stack<Token> stack, Mark mark, int index, string text)
        {
            if (stack.Peek().Mark.Fits(mark) &&!IsWhiteSpaceAfter(stack.Peek(), text) &&!IsWhiteSpaceBefore(index, text))
                foreach (var token in GetClosedTokenOrAddItToFather(stack, index))
                    yield return token;
            else
                if (stack.Peek().FatherToken != null && stack.Peek().FatherToken.Mark.Fits(mark))
                {
                    foreach (var token in GetClosedFatherToken(stack, index))
                    {
                        index += token.Mark.Sign.Length - 2;
                        yield return token;
                    }
                }
                else
                    stack.Push(new Token(index + mark.Sign.Length, mark, stack.Peek()));
        }

        private bool IsWhiteSpaceAfter(Token token, string text)
        {
            return token.StartIndex + token.Mark.Sign.Length < text.Length && 
                   Char.IsWhiteSpace(text[token.StartIndex + token.Mark.Sign.Length]);
        }

        private bool IsWhiteSpaceBefore( int index, string text)
        {
            return index > 0 && Char.IsWhiteSpace(text[index-1]);
        }

        private IEnumerable<Token> GetFirstRawToken(Stack<Token> stack, int index)
        {
            if (stack.Count != 0 && stack.Peek().Mark.Equals(Mark.RawMark))
            {
                var token = stack.Pop();
                token.SetEndIndex(index - 1);
                yield return token;
            }
        }

        private IEnumerable<Token> GetClosedTokenOrAddItToFather(Stack<Token> stack, int index)
        {
            var token = stack.Pop();
            token.SetEndIndex(index - 1);

            if (stack.Count != 0)
            {
                stack.Peek().ChildTokens.Add(token);
            }
            else
            {
                yield return token;
            }
        }

        private IEnumerable<Token> GetClosedFatherToken(Stack<Token> stack, int index)
        {
            stack.Pop();
            var token = stack.Pop();
            token.SetEndIndex(index - 1);
            yield return token;

        }

        private IEnumerable<Token> getLastRawToken(Stack<Token> stack, int index)
        {
            if (stack.Count != 0)
            {
                while (stack.Count != 1)
                {
                    stack.Pop();
                }
                var lastToken = stack.Pop();
                var token = new Token(lastToken.StartIndex - lastToken.Mark.Sign.Length, Mark.RawMark);
                token.SetEndIndex(index - 1);
                yield return token;
            }
        }

        private Mark DefineMarkAt(string text, int index, bool isBackSlashed)
        {
            if (isBackSlashed)
                return null;
            var mark = DefineDoubledMarkAt(text, index) ?? DefineSingleMarkAt(text, index);
            if (mark == null || IsBetweenDigits(mark, text, index))
                return null;
            return mark;
        }

        private  Mark DefineDoubledMarkAt(string text, int index)
        {
            if (index + 1 >= text.Length)
                return null;
            return Marks.SingleOrDefault(m =>m.Sign.Length == 2 
                                             && text[index] == m.Sign[0] 
                                             && text[index + 1] == m.Sign[1]);
        }

        private Mark DefineSingleMarkAt(string text, int index)
        {
            return Marks.SingleOrDefault(m => m.Sign.Length == 1
                                              && text[index] == m.Sign[0]);
        }

        private bool IsBetweenDigits(Mark mark,string text, int index)
        {
            return BeforeIsDigit(mark, text, index) && AfterIsDigit( mark, text, index);
        }

        private bool BeforeIsDigit(Mark mark, string text, int index)
        {
            while (index > 0)
            {
                if (Char.IsDigit(text[index - 1]))
                    return true;
                if (index - mark.Sign.Length > -1 && text.Substring(index - mark.Sign.Length, mark.Sign.Length) == mark.Sign)
                    index -= mark.Sign.Length;
                else
                    return false;
            }

            return false;
        }

        private bool AfterIsDigit(Mark mark, string text, int index)
        {
            while (index + mark.Sign.Length < text.Length)
            {
                if (Char.IsDigit(text[index + mark.Sign.Length]))
                    return true;
                if (index + 2 * mark.Sign.Length < text.Length && text.Substring(index + mark.Sign.Length, mark.Sign.Length) == mark.Sign)
                    index += mark.Sign.Length;
                else
                    return false;
            }

            return false;
        }
    }
}
