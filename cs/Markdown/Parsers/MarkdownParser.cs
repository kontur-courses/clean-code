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
    
    private sealed class ParsingContext
    {
        private readonly string text;
        
        private List<Token> tokens;

        private StringBuilder textStorage;

        private bool isHeadingOpen;
        private int emphasisOpeningTokenIndex;
        private bool emphasisStartsInsideWord;
        private bool emphasisContainsWhitespace;
        private int strongOpeningTokenIndex;
        private bool isEmphasisInvalidatedByIntersection;

        public ParsingContext(string text)
        {
            this.text = text;
            
            tokens = new List<Token>();
            textStorage = new StringBuilder();
            isHeadingOpen = false;
            emphasisOpeningTokenIndex = -1;
            emphasisStartsInsideWord = false;
            emphasisContainsWhitespace = false;
            strongOpeningTokenIndex = -1;
            isEmphasisInvalidatedByIntersection = false;
        }

        public MarkedText Parse()
        {
            for (var i = 0; i < text.Length; i++)
            {
                char? previous = i > 0 ? text[i - 1] : null;
                var current = text[i];
                char? next = i + 1 < text.Length ? text[i + 1] : null;
                
                if (emphasisOpeningTokenIndex != -1 && char.IsWhiteSpace(current))
                    emphasisContainsWhitespace = true;

                if (current == '#' && next == ' ' && (i == 0 || previous == '\n'))
                {
                    FlushText();

                    isHeadingOpen = true;
                    tokens.Add(new Token(TokenKind.HeadingStart));
                    i++;
                    continue;
                }

                if (isHeadingOpen && (current == '\n' || (current == '\r' && next == '\n')))
                {
                    FlushText();

                    tokens.Add(new Token(TokenKind.HeadingEnd));
                    isHeadingOpen = false;
                }

                if (TryProcessEscape(current, next, ref i))
                    continue;
                    
                if (current == '_' && next == '_')
                {
                    if (emphasisOpeningTokenIndex == -1 && strongOpeningTokenIndex == -1 && (i + 2 < text.Length && !char.IsWhiteSpace(text[i + 2]) && text[i + 2] != '_'))
                    {
                        FlushText();
                        strongOpeningTokenIndex = tokens.Count;
                        tokens.Add(new Token(TokenKind.Text, "__"));

                        i++;
                        continue;
                    }

                    if (strongOpeningTokenIndex != -1 && !char.IsWhiteSpace(previous ?? ' '))
                    {
                        if (emphasisOpeningTokenIndex != -1)
                        {
                            isEmphasisInvalidatedByIntersection = true;
                            strongOpeningTokenIndex = -1;
                            textStorage.Append("__");
                            i++;
                            continue;
                        }
                        FlushText();

                        tokens[strongOpeningTokenIndex] = new Token(TokenKind.StrongStart);
                        strongOpeningTokenIndex = -1;
                        tokens.Add(new Token(TokenKind.StrongEnd));

                        i++;
                        continue;
                    }
                    textStorage.Append("__");
                    i++;
                    continue;
                }

                if (current == '_')
                {
                    if (emphasisOpeningTokenIndex == -1 && !char.IsWhiteSpace(next ?? ' ') && !char.IsDigit(next ?? ' '))
                    {
                        FlushText();
                        emphasisOpeningTokenIndex = tokens.Count;
                        tokens.Add(new Token(TokenKind.Text, "_"));

                        emphasisStartsInsideWord = char.IsLetterOrDigit(previous ?? ' ');
                        emphasisContainsWhitespace = false;
                        
                        continue;
                    }

                    if (emphasisOpeningTokenIndex != -1 && !char.IsWhiteSpace(previous ?? ' ') && !char.IsDigit(previous ?? ' '))
                    {
                        var endsInsideWord = char.IsLetterOrDigit(next ?? ' ');

                        var spansDifferentWords =
                            emphasisStartsInsideWord
                            && endsInsideWord
                            && emphasisContainsWhitespace;
                        
                        if (isEmphasisInvalidatedByIntersection || spansDifferentWords)
                        {
                            textStorage.Append('_');
                            emphasisOpeningTokenIndex = -1;
                            isEmphasisInvalidatedByIntersection = false;
                            emphasisStartsInsideWord = false;
                            emphasisContainsWhitespace = false;
                            continue;
                        }
                        FlushText();

                        tokens[emphasisOpeningTokenIndex] = new Token(TokenKind.EmphasisStart);
                        tokens.Add(new Token(TokenKind.EmphasisEnd));
                        emphasisOpeningTokenIndex = -1;
                        emphasisStartsInsideWord = false;
                        emphasisContainsWhitespace = false;

                        continue;
                    }
                    textStorage.Append("_");
                    continue;
                }


                // обычный текст
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
            if (current == '\\')
            {
                if (next == '_' || next == '\\')
                {
                    textStorage.Append(next);
                    i++;
                    return true;
                }   
            }
            
            return false;
        }
        
    }
}