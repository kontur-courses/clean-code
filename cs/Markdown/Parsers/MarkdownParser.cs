using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers;

public class MarkdownParser : ITextParser
{
    public MarkedText Parse(string markdownText)
    {
        return new ParsingContext(markdownText).Parse();
    }

    private sealed class ParsingContext(string text)
    {
        private readonly StringBuilder textStorage = new();
        private readonly List<Token> tokens = [];
        private bool emphasisContainsWhitespace;
        private int emphasisOpeningTokenIndex = -1;
        private bool emphasisStartsInsideWord;
        private bool isEmphasisInvalidatedByIntersection;

        private bool isHeadingOpen;
        private int strongOpeningTokenIndex = -1;

        public MarkedText Parse()
        {
            for (var i = 0; i < text.Length; i++)
            {
                char? previous = i > 0 ? text[i - 1] : null;
                var current = text[i];
                char? next = i + 1 < text.Length ? text[i + 1] : null;
                char? nextNext = i + 2 < text.Length ? text[i + 2] : null;

                if (emphasisOpeningTokenIndex != -1 && char.IsWhiteSpace(current))
                    emphasisContainsWhitespace = true;

                if (TryOpenHeading(previous, current, next, ref i))
                    continue;

                CloseHeadingOnLineBreak(current, next);

                if (TryProcessEscape(current, next, ref i))
                    continue;

                if (TryProcessStrong(previous, current, next, nextNext, ref i))
                    continue;

                if (TryProcessEmphasis(previous, current, next))
                    continue;

                textStorage.Append(current);
            }

            FlushText();

            if (isHeadingOpen)
                tokens.Add(new Token(TokenKind.HeadingEnd));

            return new MarkedText(tokens);
        }

        private void FlushText()
        {
            if (textStorage.Length > 0)
                tokens.Add(new Token(TokenKind.Text, textStorage.ToString()));
            textStorage.Clear();
        }

        private bool TryProcessEscape(char current, char? next, ref int i)
        {
            if (current != '\\')
                return false;

            if (next is not ('_' or '\\'))
                return false;

            textStorage.Append(next);
            i++;
            return true;
        }

        private bool TryOpenHeading(char? previous, char current, char? next, ref int i)
        {
            if (current != '#')
                return false;

            if (next != ' ')
                return false;

            if (!(i == 0 || previous == '\n'))
                return false;

            FlushText();

            isHeadingOpen = true;
            tokens.Add(new Token(TokenKind.HeadingStart));

            i++;
            return true;
        }

        private void CloseHeadingOnLineBreak(char current, char? next)
        {
            if (!isHeadingOpen)
                return;

            if (!(current == '\n' || (current == '\r' && next == '\n')))
                return;

            FlushText();

            tokens.Add(new Token(TokenKind.HeadingEnd));
            isHeadingOpen = false;
        }

        private bool TryProcessStrong(char? previous, char current, char? next, char? afterDoubleUnderscore, ref int i)
        {
            if (!(current == '_' && next == '_'))
                return false;

            if (strongOpeningTokenIndex == -1)
            {
                if (char.IsWhiteSpace(afterDoubleUnderscore ?? ' ') || afterDoubleUnderscore == '_' ||
                    emphasisOpeningTokenIndex != -1)
                {
                    textStorage.Append("__");
                    i++;
                    return true;
                }

                FlushText();

                strongOpeningTokenIndex = tokens.Count;
                tokens.Add(new Token(TokenKind.Text, "__"));

                i++;
                return true;
            }

            if (char.IsWhiteSpace(previous ?? ' '))
            {
                textStorage.Append("__");
                i++;
                return true;
            }

            if (emphasisOpeningTokenIndex != -1)
            {
                isEmphasisInvalidatedByIntersection = true;
                strongOpeningTokenIndex = -1;
                textStorage.Append("__");
                i++;
                return true;
            }

            FlushText();

            tokens[strongOpeningTokenIndex] = new Token(TokenKind.StrongStart);
            strongOpeningTokenIndex = -1;
            tokens.Add(new Token(TokenKind.StrongEnd));

            i++;
            return true;
        }

        private bool TryProcessEmphasis(char? previous, char current, char? next)
        {
            if (current != '_')
                return false;

            if (emphasisOpeningTokenIndex == -1)
            {
                if (char.IsWhiteSpace(next ?? ' '))
                    return false;
                if (char.IsDigit(next ?? ' '))
                    return false;

                FlushText();

                emphasisOpeningTokenIndex = tokens.Count;
                tokens.Add(new Token(TokenKind.Text, "_"));

                emphasisStartsInsideWord = char.IsLetterOrDigit(previous ?? ' ');
                emphasisContainsWhitespace = false;

                return true;
            }

            if (char.IsWhiteSpace(previous ?? ' '))
                return false;
            if (char.IsDigit(previous ?? ' '))
                return false;

            var endsInsideWord = char.IsLetterOrDigit(next ?? ' ');

            var spansDifferentWords =
                emphasisStartsInsideWord
                && endsInsideWord
                && emphasisContainsWhitespace;

            if (isEmphasisInvalidatedByIntersection || spansDifferentWords)
            {
                textStorage.Append('_');
                ResetEmphasisState();
                return true;
            }

            FlushText();

            tokens[emphasisOpeningTokenIndex] = new Token(TokenKind.EmphasisStart);
            tokens.Add(new Token(TokenKind.EmphasisEnd));

            ResetEmphasisState();

            return true;
        }

        private void ResetEmphasisState()
        {
            emphasisOpeningTokenIndex = -1;
            isEmphasisInvalidatedByIntersection = false;
            emphasisStartsInsideWord = false;
            emphasisContainsWhitespace = false;
        }
    }
}