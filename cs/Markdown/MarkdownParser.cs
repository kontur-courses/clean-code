using Markdown.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly IMdSpecification _specification;
        private List<string> EscapeSymbols => _specification.EscapeSymbols;
        private List<string> EscapeSequence => _specification.EscapeSequences;
        private Dictionary<string, Tag> TagByMdRepresentation => _specification.TagByMdStringRepresentation;

        public MarkdownParser(IMdSpecification specification)
        {
            _specification = specification;
        }

        public List<Token> ParseToTokens(string mdText)
        {
            var stack = new Stack<(Tag tag, int index)>();
            var tokens = new List<Token>();
            var nextTokenStart = 0;
            var mdTagBuilder = new StringBuilder();

            for (int position = 0; position < mdText.Length; position++)
            {
                mdTagBuilder.Append(mdText[position]);

                if (IsPartialTag(mdText, mdTagBuilder.ToString(), position, out _))
                    continue;

                if (EscapeSymbols.Contains(mdTagBuilder.ToString()))
                    if (mdText.TrySubstring(position, 2, out string substring) && EscapeSequence.Any(s => s.StartsWith(substring)))
                    {
                        tokens.Add(new EscapeToken(position, position + 2));
                        nextTokenStart = position + 2;
                    }

                if (TagByMdRepresentation.TryGetValue(mdTagBuilder.ToString(), out Tag tag)
                    && !Escaped(mdText, position, mdTagBuilder.ToString()))
                {
                    if (!stack.IsEmpty() && tag == stack.Peek().tag)
                    {
                        if (!IsCorrectCloseTag(mdText, position, mdTagBuilder))
                        {
                            mdTagBuilder.Clear();
                            continue;
                        }
                        (Tag tag, int index) currentTag = (tag, position - tag.CloseMdTag.Length + 1);
                        if (CanUnionByToken(mdText, stack.Peek(), currentTag))
                        {
                            tokens.Add(new TagToken(stack.Pop().index + tag.OpenMdTag.Length, currentTag.index, tag));
                            nextTokenStart = position + 1;
                        }
                        else
                        {
                            var pop = stack.Pop();
                            nextTokenStart = pop.index;
                        }
                    }
                    else if (IsCorrectOpenTag(mdText, position))
                    {
                        if (stack.IsEmpty())
                            tokens.Add(new StringToken(nextTokenStart, position - tag.OpenMdTag.Length + 1));
                        (Tag tag, int index) currentTag = (tag, position - tag.OpenMdTag.Length + 1);
                        stack.Push(currentTag);
                        nextTokenStart = position;
                    }
                }
                mdTagBuilder.Clear();
            }
            if (!stack.IsEmpty())
            {
                var (_, index) = stack.Last();
                tokens.Add(new StringToken(index, mdText.Length));
            }
            else
                tokens.Add(new StringToken(nextTokenStart, mdText.Length));
            return tokens;
        }

        private bool IsCorrectOpenTag(string mdText, int position)
        {
            mdText.TryGetNextChars(position, 1, out char[] nextChars);
            if (nextChars != null && char.IsWhiteSpace(nextChars[0]))
                return false;
            return true;
        }

        private bool IsCorrectCloseTag(string mdText, int position, StringBuilder mdTagBuilder)
        {
            position = position - mdTagBuilder.Length + 1;
            mdText.TryGetCharsBehind(position, 1, out char[] behindChars);
            if (behindChars != null && char.IsWhiteSpace(behindChars[0]))
                return false;
            return true;
        }

        private bool Escaped(string mdText, int position, string mdTag)
        {
            position = position - mdTag.Length + 1;
            if (mdText.TryGetCharsBehind(position, 2, out char[] behindChars))
            {
                return behindChars[1] == '\\' && behindChars[0] != '\\';
            }
            return mdText.TryGetCharsBehind(position, 1, out char[] behindChar) && behindChar[0] == '\\';
        }

        private bool CanUnionByToken(string mdText, (Tag tag, int index) open, (Tag tag, int index) close)
        {
            var behindOpenSymbol = mdText.InRange(open.index - 1) ? mdText[open.index - 1].ToString() : null;

            var content = mdText.Substring(open.index + open.tag.OpenMdTag.Length, close.index - (open.index + open.tag.OpenMdTag.Length));

            if (content.IsNullOrWhiteSpace()) // пустая строка
                return false;
            if (content.Contains("\n")) // разные абзацы
                return false;
            if (content.ContainsDigit()) // цифры внутри
                return false;
            if (!behindOpenSymbol.IsNullOrWhiteSpace() && content.ContainsWhiteSpace()) //  разные слова
                return false;
            return true;
        }

        private bool IsPartialTag(string mdText, string mdTag, int position, out string fullTag)
        {
            if (position != mdText.LastIndex())
                fullTag = $"{mdTag}{mdText[position + 1]}";
            else fullTag = null;
            return position != mdText.LastIndex()
                && TagByMdRepresentation.Any(t => t.Key.StartsWith($"{mdTag}{mdText[position + 1]}"));
        }
    }
}
