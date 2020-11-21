using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models.Syntax;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TokenReader
    {
        private readonly ISyntax syntax;

        public TokenReader(ISyntax syntax)
        {
            this.syntax = syntax;
        }

        public List<ITaggedToken> ReadTokens(string textLine)
        {
            var allTokens = new List<ITaggedToken>();

            var textInfo = GetTextInfo(textLine);
            var clearText = textInfo.TextWithoutEscapeSymbols;
            var pairedTags = CreateTagPairs(textInfo.AllTagInfos, clearText);
            var openings = pairedTags.Where(tag => tag.IsOpening).ToList();

            openings = RemovePairsInDifferentWords(openings,
                textInfo.WhiteSpacesPositions, clearText);
            openings = RemovePairsIntersecting(openings);
            openings = ValidatePairsNesting(openings);

            pairedTags = openings
                .SelectMany(tag => new[] { tag, tag.Partner })
                .OrderBy(tag => tag.Position)
                .ToList();

            allTokens.AddRange(GetTokensFromTags(pairedTags, clearText));
            return allTokens;
        }

        private NonEscapedTextInfo GetTextInfo(string paragraph)
        {
            var tags = new List<TagInfo>();
            var textWithoutEscapeSymbols = new StringBuilder();
            var whitespacePositions = new List<int>();

            var index = 0;
            var buffer = new StringBuilder();
            var isLastEscaped = false;
            var tagsShiftsAmount = 0;

            while (index < paragraph.Length)
            {
                if (syntax.TryGetTag(paragraph, index, out var tagInfo))
                    ProcessTagAdding(tagInfo, buffer, textWithoutEscapeSymbols, tags,
                        ref isLastEscaped, ref tagsShiftsAmount);
                else
                {
                    var symbol = paragraph[index];
                    if (char.IsWhiteSpace(symbol))
                        whitespacePositions.Add(index - tagsShiftsAmount);

                    ProcessSymbolAdding(symbol, buffer, textWithoutEscapeSymbols,
                        ref isLastEscaped);
                }
                index += tagInfo?.TagLength ?? 1;
            }

            if (tags.Count > 0 && syntax.IsStartParagraphTag(tags[0].Tag))
                tags.Add(new TagInfo(new Tag(), textWithoutEscapeSymbols.Length, 0));

            return new NonEscapedTextInfo(textWithoutEscapeSymbols.ToString(),
                tags, whitespacePositions);
        }

        private void ProcessSymbolAdding(char symbol, StringBuilder buffer,
            StringBuilder textWithoutEscapeSymbols, ref bool isLastEscaped)
        {
            if (isLastEscaped)
                isLastEscaped = false;

            buffer.Append(symbol);
            textWithoutEscapeSymbols.Append(buffer);
            buffer.Clear();
        }

        private void ProcessTagAdding(TagInfo tagInfo, StringBuilder buffer,
            StringBuilder textWithoutEscapeSymbols, List<TagInfo> tags,
            ref bool isLastEscaped, ref int tagsShiftsAmount)
        {
            if (tagInfo.IsEscaped && !isLastEscaped && tagInfo.Position != 0)
            {
                isLastEscaped = true;
                buffer.Clear();
                tagsShiftsAmount++;
                textWithoutEscapeSymbols.Append(tagInfo.Tag.Opening);
            }
            else if (!syntax.IsEscapingTag(tagInfo.Tag))
            {
                tags.Add(new TagInfo(tagInfo.Tag, tagInfo.Position - tagsShiftsAmount, tagInfo.TagLength));
                textWithoutEscapeSymbols.Append(tagInfo.Tag.Opening);
                buffer.Clear();
                isLastEscaped = false;
            }
            else
            {
                buffer.Append(tagInfo.Tag.Opening);
                isLastEscaped = false;
            }
        }

        private List<PairedTag> CreateTagPairs(
            List<TagInfo> tags, string text)
        {
            if (tags.Count == 0)
                return new List<PairedTag>();

            var result = new List<PairedTag>();
            if (syntax.IsStartParagraphTag(tags[0].Tag) && syntax.IsValidAsOpening(tags[0], text))
            {
                AddPairTagsToList(tags[0], tags[^1], result);
                tags = tags.Take(tags.Count - 1).Skip(1).ToList();
            }

            var stacks = new Dictionary<string, Stack<TagInfo>>();
            foreach (var info in tags)
                stacks[info.Tag.Opening] = new Stack<TagInfo>();

            foreach (var tagInfo in tags)
            {
                var stack = stacks[tagInfo.Tag.Opening];

                var isValidAsClosing = syntax.IsValidAsClosing(tagInfo, text);
                var isLastEqual = stack.TryPeek(out var opening) &&
                                  opening.Tag.Equals(tagInfo.Tag);
                var isValid = isValidAsClosing && isLastEqual &&
                              DistanceBetweenTagsIsNotZero(opening, tagInfo) &&
                              syntax.IsValidAsOpening(opening, text);
                if (isValid)
                    AddPairTagsToList(stack.Pop(), tagInfo, result);
                else
                    stack.Push(tagInfo);
            }

            return result.OrderBy(tag => tag.Position).ToList();
        }

        private List<PairedTag> RemovePairsInDifferentWords(
            List<PairedTag> openingTags, List<int> whiteSpacesPositions, string text)
        {
            return openingTags
                .Where(tag => !AreInDifferentWords(tag, tag.Partner, whiteSpacesPositions, text))
                .ToList();
        }

        private bool AreInDifferentWords(PairedTag first, PairedTag second,
            List<int> whiteSpacesPositions, string text)
        {
            return syntax.IsInWord(first, text) && syntax.IsInWord(second, text) &&
                   whiteSpacesPositions.Count(
                       pos => first.Position < pos && pos < second.Position) > 0;
        }

        private List<PairedTag> RemovePairsIntersecting(List<PairedTag> openingTags)
        {
            if (openingTags.Count == 0)
                return openingTags;

            var result = new List<PairedTag>();
            var openedTags = new Stack<PairedTag>();
            openedTags.Push(openingTags[0]);

            foreach (var pairedTag in openingTags.Skip(1))
            {
                var IsIntersect = false;
                while (openedTags.TryPeek(out var tag) &&
                       pairedTag.Position > tag.Partner.Position)
                    result.Add(openedTags.Pop());

                while (openedTags.TryPeek(out var tag) &&
                       pairedTag.Partner.Position > tag.Partner.Position)
                {
                    IsIntersect = true;
                    openedTags.Pop();
                }

                if (!IsIntersect)
                    openedTags.Push(pairedTag);
            }
            result.AddRange(openedTags);

            return result.OrderBy(tag => tag.Position)
                .ToList();
        }

        private List<PairedTag> ValidatePairsNesting(List<PairedTag> openingTags)
        {
            var validTags = new List<PairedTag>();

            foreach (var possibleInner in openingTags)
                if (validTags.Where(tag => possibleInner.Position < tag.Partner.Position)
                    .All(tag => syntax.IsValidAsInner(tag.SourceTag, possibleInner.SourceTag)))
                    validTags.Add(possibleInner);

            return validTags.OrderBy(tag => tag.Position)
                .ToList();
        }

        private bool DistanceBetweenTagsIsNotZero(TagInfo opening, TagInfo closing)
        {
            return closing.Position - opening.Position - opening.TagLength > 0;
        }

        private void AddPairTagsToList(TagInfo first,
                TagInfo second, List<PairedTag> toAdd)
        {
            var firstPaired = new PairedTag(first.Tag, first.Position);
            var secondPaired = new PairedTag(second.Tag, second.Position);
            firstPaired.Partner = secondPaired;
            secondPaired.Partner = firstPaired;
            firstPaired.IsOpening = secondPaired.IsClosing = true;

            toAdd.Add(firstPaired);
            toAdd.Add(secondPaired);
        }

        private List<ITaggedToken> GetTokensFromTags(List<PairedTag> tags, string text)
        {
            var tokens = new List<ITaggedToken>();
            var openedTokens = new Stack<(PairedTag startTag, TaggedToken token)>();
            var lastTag = new PairedTag(new Tag(), 0);
            foreach (var currentTag in tags)
            {
                if (openedTokens.Count == 0)
                    OpenNewToken(currentTag, lastTag, tokens, openedTokens, text);
                else
                {
                    var (startTag, taggedToken) = openedTokens.Peek();

                    if (currentTag.Position == startTag.Partner.Position)
                        openedTokens.Pop().token.InnerTokens.Add(CreateTextToken(lastTag, currentTag, text));
                    else if (currentTag.Partner.Position < startTag.Partner.Position)
                    {
                        taggedToken.InnerTokens.Add(CreateTextToken(lastTag, currentTag, text));
                        var token = new TaggedToken(string.Empty, currentTag.SourceTag);
                        taggedToken.InnerTokens.Add(token);
                        openedTokens.Push((currentTag, token));
                    }
                    else
                        continue;
                }
                lastTag = currentTag;
            }
            return tokens
                .Append(CreateTextToken(lastTag, new PairedTag(new Tag(), text.Length), text))
                .Where(token => !IsEmptyTextToken(token)).ToList();
        }

        private bool IsEmptyTextToken(ITaggedToken token)
        {
            return token.GetType() == typeof(TextToken) && string.IsNullOrEmpty(token.Value);
        }

        private void OpenNewToken(PairedTag currentTag, PairedTag lastTag, List<ITaggedToken> tokens,
            Stack<(PairedTag startTag, TaggedToken token)> openedTokens, string text)
        {
            tokens.Add(CreateTextToken(lastTag, currentTag, text));
            var token = new TaggedToken(string.Empty, currentTag.SourceTag);
            openedTokens.Push((currentTag, token));
            tokens.Add(token);
        }

        private TextToken CreateTextToken(PairedTag first, PairedTag second, string text)
        {
            var firstTagLength = first.IsOpening ? first.SourceTag.Opening.Length : first.SourceTag.Closing.Length;
            var length = second.Position - first.Position - firstTagLength;
            var start = first.Position + firstTagLength;
            return new TextToken(text.Substring(start, length));
        }
    }
}
