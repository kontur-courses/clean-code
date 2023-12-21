using System.Text;
using Markdown.Converters;
using Markdown.MdParsing.Tokens;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.MdParsing
{
    public static class Md
    {
        public static readonly Dictionary<string, TagType> MdTagsTranslator = new()
        {
            { "# ", TagType.Header },
            { "__", TagType.Bold },
            { "_", TagType.Italic },
            { "\\", TagType.Escape }
        };

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

        private static List<Token> GetTokens(string paragraph)
        {
            var tokens = new List<Token>();
            var currentIndex = 0;
            while (currentIndex < paragraph.Length)
            {
                MdSymbols_Handler.HandleSymbol(tokens, paragraph, currentIndex);
                currentIndex += tokens[^1].Content.Length;
            }

            if (tokens.FirstOrDefault()?.Content == "# ")
                tokens.Add(new Token(TokenType.Tag, "# "));

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

        private static List<Token> EscapeInvalidTokens(this List<Token> tokens) =>
            tokens.Select((t, index) =>
                    t.TokenType is not TokenType.Tag || TokenValidator.IsValidTagToken(tokens, index)
                        ? t
                        : new Token(TokenType.Text, t.Content))
                .ToList();

        private static List<Token> EscapeNonPairTokens(this List<Token> tokens)
        {
            var resultTokens = new List<Token>();
            var openTags = new Stack<Token>();
            var incorrectTags = new List<Token>();
            for (var index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                resultTokens.Add(token);
                if (token.TokenType is not TokenType.Tag) continue;
                if (TokenValidator.IsTokenTagOpen(MdTagsTranslator[token.Content], tokens, index) && 
                    TokenValidator.OrderIsCorrect(openTags, token))
                {
                    openTags.Push(token);
                }
                else
                {
                    if (openTags.TryPop(out var lastOpenToken))
                        SolveOpenAndCloseTags(lastOpenToken, token, incorrectTags, openTags);
                    else
                        incorrectTags.Add(token);
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
            var openTagType = MdTagsTranslator[openToken.Content];
            var closeTagType = MdTagsTranslator[closeToken.Content];
            if (openTagType == closeTagType) return;
            incorrectTags.Add(openToken);
            incorrectTags.Add(closeToken);
        }

        private static ParsedText GetTagsAndCleanText(this List<Token> tokens)
        {
            var result = new List<ITag>();
            var paragraph = new StringBuilder();
            var isItalicOpen = false;
            var isBoldOpen = false;
            var isHeaderOpen = false;
            foreach (var token in tokens)
            {
                if (token.TokenType is not TokenType.Tag)
                {
                    paragraph.Append(token.Content);
                    continue;
                }

                CreateTag(result, token, paragraph.Length, isItalicOpen, isBoldOpen, isHeaderOpen);
                if (result[^1].Type is TagType.Italic)
                    isItalicOpen = !result[^1].IsEndTag;
                else if (result[^1].Type is TagType.Bold)
                    isBoldOpen = !result[^1].IsEndTag;
                else if (result[^1].Type is TagType.Header)
                    isHeaderOpen = !result[^1].IsEndTag;
            }

            return new ParsedText(paragraph.ToString(), result);
        }

        private static void CreateTag(List<ITag> result, Token token, int position, bool shouldCloseItalic,
            bool shouldCloseBold, bool shouldCloseHeader)
        {
            switch (MdTagsTranslator[token.Content])
            {
                case TagType.Header:
                    result.Add(new HeaderTag(position, shouldCloseHeader));
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
    }
}
