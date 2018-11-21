using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
            var sortedStartsEndEnds = tokens.SelectMany(t => new []{Tuple.Create(t.StartIndex-t.Mark.Length, t.Mark.OpeningTag, t.Mark.Sign),
                Tuple.Create(t.EndIndex+1, t.Mark.ClosingTag, t.Mark.Sign)}).OrderByDescending(el=>el);
            foreach (var pair in sortedStartsEndEnds)
            {
                builder.Remove(pair.Item1 + offset, pair.Item3.Length);
                builder.Insert(pair.Item1 + offset, pair.Item2);
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
                var childTokens = popped.ChildTokens;
                childTokens.Reverse();
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
            var builder = new StringBuilder(text);
            foreach (var oneSymbolMark in marks.Where(m => m.Length == 1))
            {
                var curIndex = 0;
                var backSlashesCount = 0;
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
