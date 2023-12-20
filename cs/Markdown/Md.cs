using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown
{
    public static class Md
    {
        private static readonly Dictionary<TagType, string> mdTags = new()
        {
            { TagType.Header, "# " },
            { TagType.Bold, "__" },
            { TagType.Italic, "_" },
            { TagType.Escape, "\\" }
        };

        private static readonly Dictionary<string, TagType> mdTagsTranslator = new()
        {
            { "# ", TagType.Header },
            { "__", TagType.Bold },
            { "_", TagType.Italic },
            { "\\", TagType.Escape }
        };

        private static readonly HashSet<char> specialSymbols = ['_', '\\', ' '];

        public static string Render(string mdString) =>
            string.Join(
                "", mdString.SplitToParagraphs()
                    .Select(ParseParagraph)
                    .Select(HtmlConverter.InsertTags)
            );

        private static ParsedText ParseParagraph(string paragraph)
        {
            return GetTokens(paragraph)
                .EscapeTags()
                .EscapeInvalidTokens()
                .EscapeNonPairTokens()
                .GetTagsAndCleanText();
        }

        public static List<Token> GetTokens(string paragraph)
        {
            var tokens = new List<Token>();
            var currentIndex = 0;
            while (currentIndex < paragraph.Length)
            {
                HandleSymbol(tokens, paragraph, currentIndex);
                currentIndex += tokens[^1].Content.Length;
            }

            return tokens;
        }

        public static List<Token> EscapeTags(this List<Token> tokens)
        {
            Token? previousToken = null;
            var result = new List<Token>();
            foreach (var token in tokens)
            {
                if (previousToken is { TokenType: TokenType.Escape })
                {
                    if (token.TokenType != TokenType.Text)
                    {
                        token.TokenType = TokenType.Text;
                        previousToken = token;
                        result.Add(token);
                    }
                    else
                    {
                        previousToken.TokenType = TokenType.Text;
                        result.Add(previousToken);
                        result.Add(token);
                        previousToken = token;
                    }
                }
                else if (token.TokenType == TokenType.Escape)
                {
                    previousToken = token;
                }
                else
                {
                    result.Add(token);
                    previousToken = token;
                }
            }

            if (previousToken is not { TokenType: TokenType.Escape }) return result;
            previousToken.TokenType = TokenType.Text;
            result.Add(previousToken);
            return result;
        }

        public static List<Token> EscapeInvalidTokens(this List<Token> tokens) =>
            tokens.Select((t, index) =>
                    t.TokenType is not TokenType.Tag || IsValidTagToken(tokens, index)
                        ? t
                        : new Token(TokenType.Text, t.Content))
                .ToList();

        private static bool IsValidTagToken(List<Token> tokens, int index)
        {
            return mdTagsTranslator[tokens[index].Content] switch
            {
                TagType.Italic => IsValidItalic(tokens, index),
                TagType.Bold => IsValidBold(tokens, index),
                _ => true
            };
        }

        private static bool IsValidBold(List<Token> tokens, int index)
        {
            return IsBoldOrItalicOpen(tokens, index) ^ IsBoldOrItalicClose(tokens, index);
        }

        private static bool IsBoldOrItalicClose(List<Token> tokens, int index)
        {
            return index - 1 > 0 && tokens[index - 1].TokenType is not TokenType.Space &&
                   (index + 1 >= tokens.Count || tokens[index + 1].TokenType is TokenType.Space);
        }
        private static bool IsBoldOrItalicOpen(List<Token> tokens, int index)
        {
            return index + 1 < tokens.Count && tokens[index + 1].TokenType is not TokenType.Space &&
                   (index - 1 < 0 || tokens[index - 1].TokenType is TokenType.Space or TokenType.Tag);
        }

        private static bool IsValidItalic(List<Token> tokens, int index)
        {
            return !(index - 1 >= 0 && index + 1 < tokens.Count &&
                     ((tokens[index - 1].TokenType is TokenType.Number &&
                       tokens[index + 1].TokenType is not TokenType.Space) ||
                      (tokens[index + 1].TokenType is TokenType.Number &&
                       tokens[index - 1].TokenType is not TokenType.Space)));
        }

        public static List<Token> EscapeNonPairTokens(this List<Token> tokens)
        {
            var resultTokens = new List<Token>();
            var openTags = new Stack<Token>();
            var incorrectTags = new List<Token>();
            for (var index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                resultTokens.Add(token);
                if (token.TokenType is TokenType.Text or TokenType.Space) continue;
                if (token.Content is "_" or "__" && IsBoldOrItalicOpen(tokens, index))
                {
                    openTags.Push(token);
                }
                else
                {
                    if (openTags.TryPop(out var lastOpenToken))
                    {
                        SolveOpenAndCloseTags(lastOpenToken, token, incorrectTags, openTags);
                    }
                    else
                    {
                        incorrectTags.Add(token);
                    }
                }
            }

            incorrectTags.AddRange(openTags);
            ChangeTypesForIncorrectTokens(incorrectTags);
            return resultTokens;
        }
        private static void ChangeTypesForIncorrectTokens(List<Token> incorrectTags)
        {
            foreach (var token in incorrectTags)
                token.TokenType = TokenType.Text;
        }

        private static void SolveOpenAndCloseTags(Token openToken, Token closeToken,
            List<Token> incorrectTags, Stack<Token> openTags)
        {
            var openTagType = mdTagsTranslator[openToken.Content];
            var closeTagType = mdTagsTranslator[closeToken.Content];
            openTags.TryPeek(out var previousOpenToken);
            TagType? previousOpenTagType = null;
            if (previousOpenToken is not null)
                previousOpenTagType = mdTagsTranslator[previousOpenToken.Content];
            if (openTagType == closeTagType) return;
            if (openTagType == TagType.Header
                && closeTagType != TagType.Header)
            {
                incorrectTags.Add(closeToken);
                openTags.Push(openToken);
            }
            else if (closeTagType == TagType.Header && previousOpenTagType == closeTagType)
            {
                incorrectTags.Add(openToken);
                openTags.Pop();
            }
            else
            {
                incorrectTags.Add(openToken);
                incorrectTags.Add(closeToken);
            }
        }

        private static ParsedText GetTagsAndCleanText(this List<Token> tokens)
        {
            var result = new List<ITag>();
            var paragraph = new StringBuilder();
            var isItalicOpen = false;
            var isBoldOpen = false;
            foreach (var token in tokens)
            {
                if (token.TokenType is TokenType.Text or TokenType.Space)
                {
                    paragraph.Append(token.Content);
                    continue;
                }

                CreateTag(result, token, paragraph.Length, isItalicOpen, isBoldOpen);
                if (result[^1].Type is TagType.Italic)
                    isItalicOpen = !result[^1].IsEndTag;
                else if (result[^1].Type is TagType.Bold)
                    isBoldOpen = !result[^1].IsEndTag;
            }

            return new ParsedText(paragraph.ToString(), result);
        }

        private static void CreateTag(List<ITag> result, Token token, int position, bool shouldCloseItalic,
            bool shouldCloseBold)
        {
            switch (mdTagsTranslator[token.Content])
            {
                case TagType.Header:
                    result.Add(new HeaderTag(position));
                    break;
                case TagType.Italic:
                    result.Add(new ItalicTag(position, shouldCloseItalic));
                    break;
                case TagType.Bold:
                    result.Add(new BoldTag(position, shouldCloseBold));
                    break;
            }
        }

        private static string[] SplitToParagraphs(this string text) => text.Split('\r', '\n');

        private static void HandleSymbol(List<Token> tokens, string paragraph, int currentIndex)
        {
            switch (paragraph[currentIndex])
            {
                case '#':
                    HandleHashSymbol(tokens, paragraph, currentIndex);
                    return;
                case '\\':
                    HandleEscapeSymbol(tokens);
                    return;
                case '_':
                    HandleUnderscore(tokens, paragraph, currentIndex);
                    return;
                case ' ':
                    HandleSpace(tokens);
                    return;
                default:
                    HandleText(tokens, paragraph, currentIndex);
                    return;
            }
        }

        private static void HandleSpace(List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.Space, " "));
        }

        private static void HandleText(List<Token> tokens, string paragraph, int currentIndex)
        {
            var stringBuilder = new StringBuilder();
            var tokenType = TokenType.Space;
            for (var i = currentIndex; i < paragraph.Length; i++)
            {
                if (tokenType == TokenType.Space)
                    tokenType = char.IsNumber(paragraph[currentIndex]) ? TokenType.Number : TokenType.Text;
                if (tokenType == TokenType.Text &&
                    (char.IsNumber(paragraph[currentIndex]) || IsSpecSymbol(paragraph[currentIndex])))
                    break;
                if (tokenType == TokenType.Number && !char.IsNumber(paragraph[currentIndex]))
                    break;
                stringBuilder.Append(paragraph[currentIndex]);
                currentIndex++;
            }

            tokens.Add(new Token(tokenType, stringBuilder.ToString()));
        }

        private static void HandleUnderscore(List<Token> tokens, string paragraph, int currentPosition)
        {
            if (currentPosition + 1 < paragraph.Length && paragraph[currentPosition + 1] == '_')
                tokens.Add(new Token(TokenType.Tag, "__"));
            else
                tokens.Add(new Token(TokenType.Tag, "_"));
        }

        private static void HandleEscapeSymbol(List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.Escape, "\\"));
        }

        private static void HandleHashSymbol(List<Token> tokens, string paragraph, int currentIndex)
        {
            if (currentIndex != 0 || 2 > paragraph.Length || paragraph[1] != ' ')
            {
                HandleText(tokens, paragraph, currentIndex);
                return;
            }

            tokens.Add(new Token(TokenType.Tag, "# "));
        }

        private static bool IsSpecSymbol(char symbol) => specialSymbols.Contains(symbol);
    }
}
