using System.Text;
using Markdown.Converters;
using Markdown.MdParsing.Tokens;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.MdParsing
{
    public static class Md
    {
        public static string Render(string mdString) =>
            new HtmlConverter().InsertTags(mdString.SplitToParagraphs()
                .Select(ParseParagraph)
                .ToArray()
            );

        private static ParsedText ParseParagraph(string paragraph)
        {
            return GetTokens(paragraph)
                .EscapeTags()
                .EscapeInvalidTokens()
                .EscapeNonPairTokens()
                .EscapeWrongOrder()
                .GetTagsAndCleanText();
        }

        private static List<Token> GetTokens(string paragraph)
        {
            var tokens = new List<Token>();
            var currentIndex = 0;
            while (currentIndex < paragraph.Length)
            {
                tokens.Add(MdTokenGenerator.GetTokenBySymbol(paragraph, currentIndex));
                currentIndex += tokens[^1].Content.Length;
            }

            if (tokens.FirstOrDefault()?.TagType is TagType.Header or TagType.BulletedList)
                tokens.Add(new Token(TokenType.Tag, tokens[0].Content, tokens[0].TagType));

            return tokens;
        }

        private static List<Token> EscapeTags(this List<Token> tokens)
        {
            Token? previousToken = null;
            var result = new List<Token>();
            foreach (var token in tokens)
            {
                if (previousToken is { TokenType: TokenType.Escape })
                {
                    if (token.TokenType is TokenType.Tag or TokenType.Escape)
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
                    previousToken = token;
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

        private static List<Token> EscapeInvalidTokens(this List<Token> tokens) =>
            tokens.Select((t, index) =>
                    t.TokenType is not TokenType.Tag || TokenValidator.IsValidTagToken(tokens, index)
                        ? t
                        : new Token(TokenType.Text, t.Content))
                .ToList();

        private static List<Token> EscapeNonPairTokens(this List<Token> tokens)
        {
            var resultTokens = new List<Token>();
            var openTagsPositions = new Stack<int>();
            var incorrectTags = new List<Token>();
            for (var index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                resultTokens.Add(token);
                if (token.TokenType is not TokenType.Tag) continue;
                if (TokenValidator.IsTokenTagOpen(token.TagType, tokens, index))
                {
                    openTagsPositions.Push(index);
                }
                else
                {
                    if (openTagsPositions.TryPop(out var lastOpenToken))
                        SolveOpenAndCloseTags(openTagsPositions, tokens, lastOpenToken, index, incorrectTags);
                    else
                        incorrectTags.Add(token);
                }
            }

            incorrectTags.AddRange(openTagsPositions.Select(x => tokens[x]));
            ChangeTypesForIncorrectTokens(incorrectTags);
            return resultTokens;
        }

        private static void ChangeTypesForIncorrectTokens(List<Token> incorrectTags)
        {
            foreach (var token in incorrectTags)
                token.TokenType = TokenType.Text;
        }

        private static void SolveOpenAndCloseTags(Stack<int> openTags, List<Token> tokens, int openIndex,
            int closeIndex, List<Token> incorrectTags)
        {
            var openTagToken = tokens[openIndex];
            var closeTagToken = tokens[closeIndex];
            closeTagToken.IsCloseTag = true;
            if (openTagToken.TagType == closeTagToken.TagType)
            {
                tokens[openIndex].PairedTagIndex = closeIndex;
                tokens[closeIndex].PairedTagIndex = openIndex;
                return;
            }

            var canGetNext = TryGetNextTagType(tokens, closeIndex, out var nextTagTokenPosition);
            if (openTags.TryPeek(out var preOpenTagIndex))
            {
                if (tokens[preOpenTagIndex].TagType != closeTagToken.TagType) return;
                if (canGetNext && !TokenValidator.IsTokenTagOpen(
                        tokens[nextTagTokenPosition].TagType,
                        tokens,
                        nextTagTokenPosition
                    ) && tokens[nextTagTokenPosition].TagType == openTagToken.TagType)
                {
                    openTags.Pop();
                    incorrectTags.Add(tokens[preOpenTagIndex]);
                    incorrectTags.Add(tokens[openIndex]);
                    incorrectTags.Add(tokens[closeIndex]);
                    return;
                }

                openTags.Pop();
                tokens[preOpenTagIndex].PairedTagIndex = closeIndex;
                tokens[closeIndex].PairedTagIndex = preOpenTagIndex;
                incorrectTags.Add(tokens[openIndex]);
                return;
            }

            if (canGetNext)
            {
                if (TokenValidator.IsTokenTagOpen(tokens[nextTagTokenPosition].TagType, tokens, nextTagTokenPosition))
                {
                    openTags.Push(openIndex);
                    incorrectTags.Add(closeTagToken);
                }
                else if (tokens[nextTagTokenPosition].TagType == tokens[openIndex].TagType)
                {
                    openTags.Push(openIndex);
                    incorrectTags.Add(closeTagToken);
                    return;
                }
            }

            incorrectTags.Add(tokens[openIndex]);
            incorrectTags.Add(tokens[closeIndex]);
        }

        private static bool TryGetNextTagType(List<Token> tokens, int index, out int nextTagToken)
        {
            for (var i = index + 1; i < tokens.Count; i++)
            {
                if (tokens[i].TokenType is not TokenType.Tag) continue;
                nextTagToken = i;
                return true;
            }

            nextTagToken = -1;
            return false;
        }

        private static List<Token> EscapeWrongOrder(this List<Token> tokens)
        {
            var result = new List<Token>();
            var openTags = new Stack<Token>();
            foreach (var t in tokens)
            {
                result.Add(t);
                if (t.TokenType is TokenType.Tag && !t.IsCloseTag)
                    openTags.Push(t);
                else if (t.TokenType is TokenType.Tag)
                    openTags.Pop();
                if (TokenValidator.OrderIsCorrect(openTags, t)) continue;
                t.TokenType = TokenType.Text;
                tokens[t.PairedTagIndex].TokenType = TokenType.Text;
            }

            return result;
        }

        private static ParsedText GetTagsAndCleanText(this List<Token> tokens)
        {
            var result = new List<ITag>();
            var paragraph = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.TokenType is not TokenType.Tag)
                {
                    paragraph.Append(token.Content);
                    continue;
                }

                result.Add(GetNewTag(token, paragraph.Length));
            }

            return new ParsedText(paragraph.ToString(), result);
        }

        private static ITag GetNewTag(Token token, int position)
        {
            return token.TagType switch
            {
                TagType.Header => new HeaderTag(position, token.IsCloseTag),
                TagType.Italic => new ItalicTag(position, token.IsCloseTag),
                TagType.Bold => new BoldTag(position, token.IsCloseTag),
                TagType.BulletedList => new BulletedListTag(position, token.IsCloseTag),
                _ => throw new NotImplementedException()
            };
        }

        private static string[] SplitToParagraphs(this string text) => text.Split('\r', '\n');
    }
}
