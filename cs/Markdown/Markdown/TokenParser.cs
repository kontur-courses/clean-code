using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenParser
    {
        public readonly IEnumerable<Mark> Marks;

        public TokenParser(IEnumerable<Mark> marks)
        {
            Marks = marks;
        }

        private int backSlashesCount = 0;

        public IEnumerable<Token> Parse(string text)
        {
            var result = new List<Token>();
            backSlashesCount = 0;
            var index = 0;
            var openedTokens = new Stack<Token>();
            while (index < text.Length)
            {
                var isBackSlashed = BackSlashesCountIsOddBeforeUpdate(text, index);
                var mark = DefineMarkAt(text, index, isBackSlashed);
                if (mark != null)
                {
                    AddPreviousRawToken(openedTokens, result, index);
                    if (IsClosing(openedTokens, mark, text, index))
                        AddToFatherOrToResult(openedTokens, result, mark, index);
                    else
                        PushTokenIfCanBeOpening(openedTokens, mark, text, index);
                    index += mark.Length;
                }
                else
                {
                    if (openedTokens.Count==0)
                        openedTokens.Push(new Token(index, Mark.RawMark));
                    index++;
                }
            }
            AddAllRemainingTokensAsRowToken(result, openedTokens, index);
            return result;
        }


        private void PushTokenIfCanBeOpening(Stack<Token> openedTokens, Mark mark, string text, int index)
        {
            var fatherToken = openedTokens.Count == 0 ? null : openedTokens.Peek();
            var token = new Token(index+mark.Length, mark,fatherToken);
            var indexAfterMark = token.StartIndex + token.Mark.Length;
            if (indexAfterMark >= text.Length || indexAfterMark < text.Length  && !char.IsWhiteSpace(text[indexAfterMark]))
                openedTokens.Push(token);
        }

        private void AddToFatherOrToResult(Stack<Token> openedTokens, List<Token> result, Mark mark, int index)
        {
            var previousToken = openedTokens.Pop();
            while (!previousToken.Mark.Fits(mark))
                previousToken = openedTokens.Pop();
            previousToken.SetEndIndex(index-1);
            if (previousToken.FatherToken == null)
                result.Add(previousToken);
            else
                previousToken.FatherToken.ChildTokens.Add(previousToken);
        }

        private bool IsClosing(Stack<Token> openedTokens, Mark mark, string text, int index)
        {
            var indexBeforeMark = index -1;
            var afterIsWhiteSpace = indexBeforeMark > 0 && char.IsWhiteSpace(text[indexBeforeMark]);
            if (afterIsWhiteSpace || openedTokens.Count==0)
                return false;
            var previousToken = openedTokens.Peek();
            while (previousToken != null)
            {
                if (previousToken.Mark.Fits(mark) && previousToken.StartIndex!=index)
                    return true;
                previousToken = previousToken.FatherToken;
            }
            return false;
        }

        private void AddPreviousRawToken(Stack<Token> stack, List<Token> result, int index)
        {
            if (stack.Count == 0 || !stack.Peek().Mark.Equals(Mark.RawMark))
                return;
            var token = stack.Pop();
            token.SetEndIndex(index - 1);
            result.Add(token);
        }

        private bool BackSlashesCountIsOddBeforeUpdate(string text, int index)
        {
            if (text[index] == '\\')
                backSlashesCount++;
            var isOdd = backSlashesCount % 2 == 1;
            if (text[index] != '\\')
                backSlashesCount = 0;
            return isOdd;
        }


        private void AddAllRemainingTokensAsRowToken(List<Token> result, Stack<Token> openedTokens, int index)
        {
            if (openedTokens.Count == 0)
                return;
            while (openedTokens.Count != 1)
            {
                openedTokens.Pop();
            }
            var lastToken = openedTokens.Pop();
            var token = new Token(lastToken.StartIndex - lastToken.Mark.Length, Mark.RawMark);
            token.SetEndIndex(index - 1);
            result.Add(token);
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
                if (char.IsDigit(text[index - 1]))
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
                if (char.IsDigit(text[index + mark.Sign.Length]))
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
