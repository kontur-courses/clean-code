using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly string text;
        public int PositionInText { get; private set; }
        private Stack<Token> tagStack;
        private readonly MarkupProcessor markupProcessor;

        public Tokenizer(string text, MarkupProcessor markupProcessor)
        {
            this.text = text;
            tagStack = new Stack<Token>();
            this.markupProcessor = markupProcessor;
        }


        public static bool IsClosing(string paragraph, string tag, int tagPosition, Token lastTag)
        {
            return !IsAfterWhiteSpace(paragraph, tagPosition)
                   && !IsInsideNumber(paragraph, tagPosition, tag.Length)
                   && paragraph.Substring(lastTag) == tag;
        }

        public static bool IsOpening(string paragraph, string tag, int tagPosition, Token lastTag)
        {
            return !IsBeforeWhiteSpace(paragraph, tagPosition, tag.Length)
                   && !IsInsideNumber(paragraph, tagPosition, tag.Length);
        }

        public Token ReadTag(string paragraph, int positionInParagraph)
        {
            TryReadTag(paragraph, positionInParagraph, out var token, out _);
            if (token == null)
                return null;
            if (IsClosing(paragraph, text.Substring(token), positionInParagraph, tagStack.FirstOrDefault()))
                tagStack.Pop();
            else
                tagStack.Push(token);

            return token;
        }

        public bool TryReadTag(string paragraph, int positionInParagraph, out Token result, out int longestTagLen)
        {
            var lastTag = tagStack.FirstOrDefault();
            result = null;
            longestTagLen = 0;
            if (Escaped(paragraph, positionInParagraph))
                return false;
            var possibleTags = markupProcessor.AllTags
                .Where(tag => positionInParagraph + tag.Length <= paragraph.Length)
                .Where(tag => paragraph.Substring(positionInParagraph, tag.Length) == tag);

            if (!possibleTags.Any())
                return false;
            var longestTag = possibleTags
                .Aggregate("", (cur, max) => cur.Length > max.Length ? cur : max);
            longestTagLen = longestTag.Length;
            if (!IsClosing(paragraph, longestTag, positionInParagraph, lastTag) &&
                !IsOpening(paragraph, longestTag, positionInParagraph, lastTag))
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
            if (secondTag == null)
                return false;
            var currentTagText = text.Substring(firstTag);
            var lastTagText = text.Substring(secondTag);
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

        private static bool Escaped(string paragraph, int position)
        {
            if (position == 0)
                return false;
            var i = position - 1;
            var slashesCount = 0;
            while (paragraph[i] == '\\')
            {
                slashesCount++;
                i -= 1;
            }

            return slashesCount % 2 != 0;
        }

        public static bool IsInsideNumber(string paragraph, int tagPosition, int tagLen)
        {
            if (tagPosition == 0 || tagPosition + tagLen >= paragraph.Length)
                return false;
            return char.IsDigit(paragraph[tagPosition - 1]) && char.IsDigit(paragraph[tagPosition + tagLen]);
        }

        private static bool IsAfterWhiteSpace(string paragraph, int tagPosition)
        {
            if (tagPosition == 0)
                return true;
            return paragraph[tagPosition - 1] == ' ';
        }

        private static bool IsBeforeWhiteSpace(string paragraph, int tagPosition, int tagLen)
        {
            if (tagPosition + tagLen >= paragraph.Length)
                return true;
            return paragraph[tagPosition + tagLen] == ' ';
        }

        private static bool IsInsideWord(string paragraph, int tagPosition, int tagLen)
        {
            var isLetterOnTheLeft = tagPosition == 0 || char.IsLetter(paragraph[tagPosition - 1]);
            var isLetterOnTheRight = tagPosition + tagLen < paragraph.Length
                                     && char.IsLetter(paragraph[tagPosition + tagLen]);
            return isLetterOnTheLeft && isLetterOnTheRight;
        }

        public IEnumerable<string> GetParagraphs()
        {
            return text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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

            PositionInText += paragraph.Length + Environment.NewLine.Length;
            while (tagStack.Any())
            {
                var tag = tagStack.Pop();
                if (!markupProcessor.SingleTags.Contains(paragraph.Substring(tag)))
                    tag.IsMarkup = false;
            }

            return tokens;
        }

        public void TryAddNewlineAfterParagraph(List<Token> tokens)
        {
            if (tokens[tokens.Count - 1].End + Environment.NewLine.Length < text.Length)
                tokens[tokens.Count - 1].Length += Environment.NewLine.Length;
        }

        public void RemoveEmptyMarkupTokens(List<Token> tokens)
        {
            tokens = tokens.Where(token => token.Length != 0).ToList();

            var prevToken = tokens[0];
            foreach (var curToken in tokens.Skip(1))
            {
                if (curToken.IsMarkup
                    && prevToken.IsMarkup
                    && text.Substring(curToken) == text.Substring(prevToken))
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