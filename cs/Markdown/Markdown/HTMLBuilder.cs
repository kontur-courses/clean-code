using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class HtmlBuilder
    {
        public static string Build(IEnumerable<Token> tokens, string text)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Mark.Equals(Mark.RawMark))
                    builder.Append(text.Substring(token.StartIndex, token.EndIndex - token.StartIndex + 1));
                else
                    builder.Append(AppendTaggedToken(text, token));
            }
            return builder.ToString();
        }

        private static string AppendTaggedToken(string text, Token token)
        {
            var builder = new StringBuilder();
            builder.Append(token.Mark.OpeningTag);
            builder.Append(text.Substring(token.StartIndex, token.EndIndex - token.StartIndex + 1));
            builder.Append(token.Mark.ClosingTag);
            var offset = token.Mark.OpeningTag.Length - token.Mark.Sign.Length;
            var tokens = GetAllChildTokens(token);
            foreach (var t in tokens)
            {
                builder.Remove(t.StartIndex - t.Mark.Sign.Length+offset, t.Mark.Sign.Length);
                offset -= t.Mark.Sign.Length;
                builder.Insert(t.StartIndex+offset, t.Mark.OpeningTag);
                offset += t.Mark.OpeningTag.Length;

                builder.Remove(t.EndIndex +1 + offset, t.Mark.Sign.Length);
                builder.Insert(t.EndIndex + 1 + offset, t.Mark.ClosingTag);
                offset += t.Mark.ClosingTag.Length-t.Mark.Sign.Length;
            }

            return builder.ToString();
        }

        private static IEnumerable<Token> GetAllChildTokens(Token token)
        {
            
            var stack = new Stack<Token>();
            stack.Push(token);
            while (stack.Count != 0)
            {
                var popped = stack.Pop();
                foreach (var childToken in popped.ChildTokens)
                {
                    stack.Push(childToken);
                }

                if (!popped.Equals(token))
                {
                    yield return popped;
                }   
            }
        }


        public static string RemoveRedundantBackSlashes(string text, IEnumerable<Mark> marks)
        {
            var backSlashesCount = 0;
            var builder = new StringBuilder(text);
            var curIndex = 0;
            foreach (var oneSymbolMark in marks.Where(m => m.Sign.Length == 1))
            {
                while (curIndex < builder.Length)
                {
                    if (builder[curIndex] == '\\')
                        backSlashesCount++;
                    else
                    {
                        if (builder[curIndex].ToString() == oneSymbolMark.Sign && backSlashesCount % 2 == 1)
                        {
                            builder.Remove(curIndex - 1, 1);
                            curIndex--;
                        }
                        backSlashesCount = 0;
                    }
                    curIndex++;
                }
            }
            builder.Replace(@"\\", @"\");
            return builder.ToString();
        }
    }
}
