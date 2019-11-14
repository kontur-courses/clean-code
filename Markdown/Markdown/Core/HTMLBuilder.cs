using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HTMLBuilder
    {
        public static string Build(IEnumerable<Token> tokens, string text, IEnumerable<MarkTranslatorElement> marks, char escapeCharacter)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                builder.Append(AppendTaggedToken(text, token));
            }
            return RemoveRedundantEscapeCharacters(builder.ToString(), marks, escapeCharacter);
        }

        private static string AppendTaggedToken(string text, Token token)
        {
            var builder = new StringBuilder();
            builder.Append(token.MarkToTranslate.To.Opening);
            builder.Append(token.Value);
            builder.Append(token.MarkToTranslate.To.Closing);
            var offset = token.MarkToTranslate.To.Opening.Length - token.MarkToTranslate.From.Opening.Length;
            var tokens = GetAllChildTokens(token);
            var sortedStartsEndEnds = tokens
                .SelectMany(t => new []{Tuple.Create(t.StartIndex-t.MarkToTranslate.From.Opening.Length,
                        t.MarkToTranslate.To.Opening, t.MarkToTranslate.From.Opening),
                Tuple.Create(t.EndIndex+1, t.MarkToTranslate.To.Closing, t.MarkToTranslate.From.Closing)})
                .OrderByDescending(item=>item);
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


        private static string RemoveRedundantEscapeCharacters(string text, IEnumerable<MarkTranslatorElement> marks, char escapeCharacter)
        {
            var builder = new StringBuilder(text);
            foreach (var oneSymbolMark in marks.Where(m => m.From.Opening.Length == 1))
            {
                var curIndex = 0;
                var escapeCharactersCount = 0;
                while (curIndex < builder.Length)
                {
                    if (builder[curIndex].Equals(escapeCharacter))
                        escapeCharactersCount++;
                    else
                    {
                        if (builder[curIndex].ToString() == oneSymbolMark.From.Opening && escapeCharactersCount % 2 == 1)
                        {
                            builder.Remove(curIndex - 1, 1);
                            curIndex--;
                        }
                        escapeCharactersCount = 0;
                    }
                    curIndex++;
                }
            }
            builder.Replace(@"\\", @"\");
            return builder.ToString();
        }
    }
}

        
    