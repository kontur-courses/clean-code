using System;
using System.Collections.Generic;
using System.Linq;
using static Markdown.NewLineHandler;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly string text;
        public int PositionInText { get; private set; }
        private Stack<Token> tagStack;
        private readonly MarkupProcessor markupProcessor;
        private char EscapeSymbol { get; }

        public Tokenizer(string text, MarkupProcessor markupProcessor, char escapeSymbol = '\\')
        {
            this.text = text;
            tagStack = new Stack<Token>();
            this.markupProcessor = markupProcessor;
            EscapeSymbol = escapeSymbol;
        }


        public static bool IsClosing(string paragraph, string tag, int tagPosition, Token lastTag)
        {
            return !IsAfterWhiteSpace(paragraph, tagPosition)
                   && !IsInsideNumber(paragraph, tagPosition, tag.Length)
                   && paragraph.GetTokenText(lastTag) == tag;
        }

        public static bool IsOpening(string paragraph, string tag, int tagPosition)
        {
            return !IsBeforeWhiteSpace(paragraph, tagPosition, tag.Length)
                   && !IsInsideNumber(paragraph, tagPosition, tag.Length);
        }

        public Token ReadTag(string paragraph, int positionInParagraph)
        {
            if (!TryReadTag(paragraph, positionInParagraph, out var token, out _))
                return null;
            if (IsClosing(text, text.GetTokenText(token), positionInParagraph, tagStack.FirstOrDefault()))
                tagStack.Pop();
            else
                tagStack.Push(token);

            return token;
        }

        public bool TryReadTag(string paragraph, int positionInParagraph, out Token result, out int longestTagLen)
        {
            result = null;
            longestTagLen = 0;
            if (Escaped(paragraph, positionInParagraph))
                return false;
            var possibleTags = markupProcessor.AllTags
                .Where(tag => positionInParagraph + tag.Length <= paragraph.Length)
                .Where(tag => paragraph.Substring(positionInParagraph, tag.Length) == tag)
                .Where(tag => positionInParagraph == 0 && markupProcessor.IsStartingTag(tag) ||
                              positionInParagraph != 0 && !markupProcessor.IsSingleTag(tag));

            if (!possibleTags.Any())
                return false;
            var longestTag = possibleTags
                .OrderByDescending(tag => tag.Length)
                .First();
            longestTagLen = longestTag.Length;
            var lastTag = tagStack.FirstOrDefault();
            if (!IsClosing(text, longestTag, positionInParagraph, lastTag) &&
                !IsOpening(text, longestTag, positionInParagraph))
            {
                return false;
            }

            if (lastTag != null
                && IsInsideWord(paragraph, positionInParagraph, longestTagLen)
                && IsInsideWord(paragraph, lastTag.Start - PositionInText, lastTag.Length))
            {
                return false;
            }

            var tagToken = new Token(PositionInText + positionInParagraph, longestTag.Length);
            if (!CheckTagsOverlap(tagToken, lastTag))
                tagToken.IsMarkup = true;
            if (tagToken.IsMarkup)
                result = tagToken;

            return tagToken.IsMarkup;
        }

        public bool CheckTagsOverlap(Token firstTag, Token secondTag)
        {
            if (firstTag == null || secondTag == null)
                return false;
            var currentTagText = text.GetTokenText(firstTag);
            var lastTagText = text.GetTokenText(secondTag);
            return lastTagText != currentTagText && currentTagText.Contains(lastTagText);
        }

        public Token ReadTextUntilTagOrWhiteSpace(string paragraph, int positionInParagraph)
        {
            var startingPosition = positionInParagraph;
            while (true)
            {
                if (positionInParagraph >= paragraph.Length)
                    break;
                if (TryReadTag(paragraph, positionInParagraph, out _, out var longestTagLen))
                    break;
                if (paragraph[positionInParagraph] == ' ')
                {
                    positionInParagraph++;
                    break;
                }

                positionInParagraph = Math.Min(paragraph.Length, positionInParagraph + longestTagLen + 1);
            }

            var token = new Token(PositionInText + startingPosition, positionInParagraph - startingPosition);
            return token;
        }

        private bool Escaped(string paragraph, int position)
        {
            if (position <= 0)
                return false;
            var i = position - 1;
            var slashesCount = 0;
            while (paragraph[i] == EscapeSymbol)
            {
                slashesCount++;
                i--;
            }

            return slashesCount % 2 != 0;
        }

        public static bool IsInsideNumber(string paragraph, int tagPosition, int tagLength)
        {
            var positionAfterTag = tagPosition + tagLength;
            if (tagPosition <= 0 || positionAfterTag >= paragraph.Length)
                return false;
            return char.IsDigit(paragraph[tagPosition - 1]) && char.IsDigit(paragraph[positionAfterTag]);
        }

        private static bool IsAfterWhiteSpace(string paragraph, int tagPosition)
        {
            if (tagPosition <= 0)
                return true;
            return paragraph[tagPosition - 1] == ' ';
        }

        private static bool IsBeforeWhiteSpace(string paragraph, int tagPosition, int tagLength)
        {
            var positionAfterTag = tagPosition + tagLength;
            if (positionAfterTag >= paragraph.Length)
                return true;
            return paragraph[positionAfterTag] == ' ';
        }

        private static bool IsInsideWord(string paragraph, int tagPosition, int tagLength)
        {
            var isLetterOnTheLeft = tagPosition == 0 || char.IsLetter(paragraph[tagPosition - 1]);
            var positionAfterTag = tagPosition + tagLength;
            var isLetterOnTheRight = positionAfterTag < paragraph.Length
                                     && char.IsLetter(paragraph[positionAfterTag]);
            return isLetterOnTheLeft && isLetterOnTheRight;
        }

        public string[] GetParagraphs()
        {
            return text.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<Token> TokenizeParagraph(string paragraph)
        {
            var positionInParagraph = 0;
            var tokens = new List<Token>();
            tagStack = new Stack<Token>();
            while (positionInParagraph < paragraph.Length)
            {
                var tagToken = ReadTag(paragraph, positionInParagraph);
                if (tagToken != null)
                {
                    tokens.Add(tagToken);
                    positionInParagraph += tagToken.Length;
                }

                var textToken = ReadTextUntilTagOrWhiteSpace(paragraph, positionInParagraph);
                tokens.Add(textToken);
                positionInParagraph += textToken.Length;
            }

            TryAddNewlineAfterParagraph(tokens);
            PositionInText += paragraph.Length;
            if (TryGetNewLineSymbolAtPosition(text, out var newLineSymbol, PositionInText))
                PositionInText += newLineSymbol.Length;
            while (tagStack.Any())
            {
                var tag = tagStack.Pop();
                if (!markupProcessor.IsSingleTag(text.GetTokenText(tag)))
                    tag.IsMarkup = false;
            }

            return tokens;
        }

        public void TryAddNewlineAfterParagraph(List<Token> tokens)
        {
            if (tokens.Count == 0)
                return;
            var lastToken = tokens[tokens.Count - 1];
            if (TryGetNewLineSymbolAtPosition(text, out var newLineSymbol, lastToken.End + 1))
                lastToken.Length += newLineSymbol.Length;
        }

        public void RemoveEmptyMarkupTokens(List<Token> tokens)
        {
            tokens = tokens.Where(token => token.Length != 0).ToList();

            var prevToken = tokens[0];
            foreach (var curToken in tokens.Skip(1))
            {
                if (curToken.IsMarkup
                    && prevToken.IsMarkup
                    && text.GetTokenText(curToken) == text.GetTokenText(prevToken))
                {
                    curToken.IsMarkup = false;
                    prevToken.IsMarkup = false;
                }

                prevToken = curToken;
            }
        }

        public IEnumerable<Token> GetTokens()
        {
            var tokens = new List<Token>();
            foreach (var paragraph in GetParagraphs())
            {
                tokens.AddRange(TokenizeParagraph(paragraph));
            }

            RemoveEmptyMarkupTokens(tokens);
            return tokens;
        }
    }
}