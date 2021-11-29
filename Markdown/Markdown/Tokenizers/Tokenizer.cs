using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Tokenizers
{
    public class Tokenizer : ITokenizer
    {
        // Можно не делать поле, а просто сделать все методы связанные с применением правил разметки статическими,
        // но тогда придется передавать туда сюда ссылку и постоянно создавать новые списки и добавлять туда 80%
        // одинаковых элементов, не знаю есть ли в этом смысл, разве что только со стороны читаемости, но думаю что
        // все и так выглядит неплохо (по сравнению с тем, что было изначально)
        // На данный момент статическими методами являются только те, которые как-то изменяют количество токенов
        private List<Token> tokens;

        private static TokenType GetTokenTypePair(TokenType type) =>
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            type switch
            {
                TokenType.BoldOpened => TokenType.BoldClosed,
                TokenType.ItalicOpened => TokenType.ItalicClosed,
                _ => throw new ArgumentException("Unexpected type")
            };

        private static Token TokenizeLexeme(string lexeme) =>
            lexeme switch
            {
                "_" => new Token(TokenType.ItalicOpened, lexeme),
                "__" => new Token(TokenType.BoldOpened, lexeme),
                "# " => new Token(TokenType.HeaderOpened, lexeme),
                "\\" => new Token(TokenType.EscapeSymbol, lexeme),
                _ => new Token(TokenType.Word, lexeme)
            };

        public IEnumerable<Token> Tokenize(IEnumerable<string> lexemes)
        {
            tokens = lexemes.Select(TokenizeLexeme).ToList();

            tokens = ResolveEscapeSymbols(tokens);

            ResolvePairedTags(TokenType.ItalicOpened);

            ChangeToWordsUnpairedTags(TokenType.ItalicOpened);

            ResolvePairedTags(TokenType.BoldOpened);

            ChangeToWordsUnpairedTags(TokenType.BoldOpened);

            ResolveBoldAndItalicIntersections();

            ResolveBoldInsideItalic();

            tokens = ResolveHeaders(tokens);

            ResolvePairedTagsWithEmptyStringBeetween();

            return tokens;
        }

        private static List<Token> ResolveEscapeSymbols(IReadOnlyList<Token> tokens)
        {
            var resolvedTokens = new List<Token>();
            for (var i = 0; i < tokens.Count; ++i)
            {
                if (tokens[i].Type is TokenType.EscapeSymbol &&
                    i + 1 < tokens.Count &&
                    tokens[i + 1].Type is not TokenType.Word)
                {
                    resolvedTokens.Add(new Token(TokenType.Word, tokens[i + 1].Value));
                }
                else
                {
                    resolvedTokens.Add(tokens[i]);
                }
            }

            return resolvedTokens;
        }

        private static List<Token> ResolveHeaders(IReadOnlyList<Token> tokens)
        {
            var resolvedTokens = new List<Token>();
            var isHeaderOpened = false;

            for (var i = 0; i < tokens.Count; ++i)
            {
                if (tokens[i].Type is TokenType.HeaderOpened)
                {
                    if (!isHeaderOpened &&
                        (i > 0 && tokens[i - 1].Value == "\n" || i == 0))
                    {
                        resolvedTokens.Add(tokens[i]);
                        isHeaderOpened = true;
                    }
                    else
                    {
                        resolvedTokens.Add(new Token(TokenType.Word, tokens[i].Value));
                    }
                }
                else if (isHeaderOpened && tokens[i].Value == "\n")
                {
                    resolvedTokens.Add(new Token(TokenType.HeaderClosed, ""));
                    resolvedTokens.Add(tokens[i]);
                    isHeaderOpened = false;
                }
                else
                {
                    resolvedTokens.Add(tokens[i]);
                }
            }

            if (isHeaderOpened)
            {
                resolvedTokens.Add(new Token(TokenType.HeaderClosed, ""));
            }

            return resolvedTokens;
        }

        private void ResolvePairedTags(TokenType openingTag)
        {
            for (var i = 0; i < tokens.Count; ++i)
            {
                // ReSharper disable once InvertIf
                if (tokens[i].Type == openingTag)
                {
                    if (CouldBeAnOpeningTag(i))
                    {
                        var pair = FindTagPair(i, openingTag);

                        if (pair == -1)
                        {
                            tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                        }
                        else
                        {
                            tokens[pair] = new Token(GetTokenTypePair(openingTag), tokens[pair].Value);
                            i = pair;
                        }
                    }
                    else
                    {
                        tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                    }
                }
            }
        }

        private void ChangeToWordsUnpairedTags(TokenType type)
        {
            var isOpened = false;
            for (var i = 0; i < tokens.Count; ++i)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (tokens[i].Type == type)
                {
                    if (isOpened)
                    {
                        tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                    }
                    else
                    {
                        isOpened = true;
                    }
                }
                else if (tokens[i].Type == GetTokenTypePair(type))
                {
                    isOpened = false;
                }
            }
        }

        private int FindTagPair(int index, TokenType openingTag)
        {
            var startInWord = IsInWord(index);
            var isAnotherWord = false;
            index++;
            while (index < tokens.Count &&
                   tokens[index].Value != "\n")
            {
                if (string.IsNullOrWhiteSpace(tokens[index].Value))
                {
                    if (startInWord)
                    {
                        return -1;
                    }

                    isAnotherWord = true;
                }

                if (tokens[index].Type == openingTag &&
                    CouldBeAClosingTag(index) &&
                    (IsInWord(index) && !isAnotherWord || !IsInWord(index)))
                {
                    return index;
                }

                ++index;
            }

            return -1;
        }

        private void ResolveBoldAndItalicIntersections()
        {
            var stack = new Stack<int>();

            for (var i = 0; i < tokens.Count; ++i)
            {
                if (tokens[i].Type is not
                    (TokenType.ItalicOpened or TokenType.BoldOpened or TokenType.ItalicClosed or TokenType.BoldClosed))
                {
                    continue;
                }

                if (stack.Count == 0)
                {
                    // ReSharper disable once ConvertIfStatementToSwitchStatement
                    if (tokens[i].Type is TokenType.ItalicOpened or TokenType.BoldOpened)
                    {
                        stack.Push(i);
                    }
                    else if (tokens[i].Type is TokenType.ItalicClosed or TokenType.BoldClosed)
                    {
                        tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                    }
                }
                else if (GetTokenTypePair(tokens[stack.Peek()].Type) == tokens[i].Type)
                {
                    stack.Pop();
                }
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                else if (stack.Count < 2)
                {
                    stack.Push(i);
                }
                else if (stack.Count == 2)
                {
                    while (stack.Count > 0)
                    {
                        var index = stack.Pop();
                        tokens[index] = new Token(TokenType.Word, tokens[index].Value);
                    }

                    tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                }
            }

            while (stack.Count > 0)
            {
                var index = stack.Pop();
                tokens[index] = new Token(TokenType.Word, tokens[index].Value);
            }
        }

        private void ResolveBoldInsideItalic()
        {
            var italicOpened = false;
            for (var i = 0; i < tokens.Count; ++i)
            {
                if (tokens[i].Type is TokenType.ItalicOpened or TokenType.ItalicClosed)
                {
                    italicOpened = !italicOpened;
                }

                if (italicOpened && tokens[i].Type is TokenType.BoldOpened or TokenType.BoldClosed)
                {
                    tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                }
            }
        }

        private void ResolvePairedTagsWithEmptyStringBeetween()
        {
            for (var i = 0; i < tokens.Count - 1; ++i)
            {
                // ReSharper disable once InvertIf
                if (tokens[i].Type is TokenType.ItalicOpened or TokenType.BoldOpened &&
                    tokens[i + 1].Type == GetTokenTypePair(tokens[i].Type))
                {
                    tokens[i] = new Token(TokenType.Word, tokens[i].Value);
                    tokens[i + 1] = new Token(TokenType.Word, tokens[i].Value);
                }
            }
        }

        private bool CouldBeATag(int index)
        {
            return !(index < tokens.Count - 1 && int.TryParse(tokens[index + 1].Value, out _)) &&
                   !(index > 0 && int.TryParse(tokens[index - 1].Value, out _));
        }

        private bool CouldBeAnOpeningTag(int index)
        {
            return CouldBeATag(index) &&
                   !(index < tokens.Count - 1 && string.IsNullOrWhiteSpace(tokens[index + 1].Value));
        }

        private bool CouldBeAClosingTag(int index)
        {
            return CouldBeATag(index) &&
                   !(index > 0 && string.IsNullOrWhiteSpace(tokens[index - 1].Value));
        }

        private bool IsInWord(int index)
        {
            if (index == 0 || index == tokens.Count - 1)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(tokens[index - 1].Value) &&
                   !string.IsNullOrWhiteSpace(tokens[index + 1].Value);
        }
    }
}