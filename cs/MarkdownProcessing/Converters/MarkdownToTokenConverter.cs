using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownProcessing.Tags;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Converters
{
    public class MarkdownToTokenConverter
    {
        private readonly string input;
        private string tagToCheck;
        private StringBuilder currentPossibleTag;
        private StringBuilder currentPossiblePhrase;
        public readonly Stack<Token> AllTokens;

        public MarkdownToTokenConverter(string input)
        {
            this.input = input ?? throw new ArgumentException();
            currentPossibleTag = new StringBuilder();
            currentPossiblePhrase = new StringBuilder();
            AllTokens = new Stack<Token>();
            AllTokens.Push(new ComplicatedToken(TokenType.Parent));
        }

        public string ParseInputIntoTokens()
        {
            for (var index = 0; index < input.Length; index++)
            {
                var symbol = input[index];
                AppendSymbolToCurrentPhrases(symbol);
                if (IsNotTheTagPart(symbol))
                    currentPossibleTag = new StringBuilder();
                CheckForOpeningTag();
                CheckForClosingTag();
                if (index != input.Length - 1) continue;
                if (currentPossiblePhrase.Length == 0) continue;
                AddSimpleTokenTo(AllTokens.Peek() as ComplicatedToken);
                RefreshTagAndPhraseBuffers();
            }

            return MakeHtmlFromMarkdown();
        }

        private void AppendSymbolToCurrentPhrases(char symbol)
        {
            currentPossibleTag.Append(symbol);
            currentPossiblePhrase.Append(symbol);
        }

        private bool IsNotTheTagPart(char symbol) =>
            !MarkdownTags.TagSymbols.Contains(symbol);

        private void CheckForOpeningTag()
        {
            tagToCheck = GetTagFromStringBuilder();
            if (!MarkdownTags.PossibleTags.ContainsKey(tagToCheck)) return;
            if (TagIsRepeating()) return;
            if (CurrentPhraseIsNotZero())
                AddSimpleTokenTo(AllTokens.Peek() as ComplicatedToken);
            AllTokens.Push(new ComplicatedToken(MarkdownTags.PossibleTags[tagToCheck]));
            RefreshTagAndPhraseBuffers();
        }

        private string GetTagFromStringBuilder() =>
            currentPossibleTag.Length > 1
                ? currentPossibleTag.ToString().Substring(0, currentPossibleTag.Length - 1)
                : currentPossibleTag.ToString();

        private bool TagIsRepeating() => AllTokens.Peek().Type == MarkdownTags.PossibleTags[tagToCheck];

        private bool CurrentPhraseIsNotZero() =>
            currentPossiblePhrase.Remove(currentPossiblePhrase.Length - tagToCheck.Length, tagToCheck.Length)
                .Length != 0;

        private void RefreshTagAndPhraseBuffers()
        {
            currentPossibleTag = new StringBuilder();
            currentPossiblePhrase = new StringBuilder();
        }

        private void CheckForClosingTag()
        {
            if (AllTokens.Count == 1) return;
            tagToCheck = GetTagFromStringBuilder();
            if (!MarkdownTags.PossibleTags.ContainsKey(tagToCheck)) return;
            if (ClosingTagIsNotTheSame()) return;
            if (!CurrentPhraseIsNotZero())
            {
                var token = AllTokens.Pop();
                tagToCheck += currentPossibleTag;
                if (MarkdownTags.PossibleTags.ContainsKey(tagToCheck))
                    if (!ClosingTagIsNotTheSame())
                        AddTokenToParentToken();
                    else AllTokens.Push(new ComplicatedToken(MarkdownTags.PossibleTags[tagToCheck]));
                else AllTokens.Push(token);
            }
            else AddTokenToParentToken();

            RefreshTagAndPhraseBuffers();
        }

        private bool ClosingTagIsNotTheSame() => AllTokens.Peek().Type != MarkdownTags.PossibleTags[tagToCheck];

        private void AddTokenToParentToken()
        {
            var token = AllTokens.Pop() as ComplicatedToken;
            var parentToken = AllTokens.Peek() as ComplicatedToken;
            parentToken?.ChildTokens.Add(token);
            AddSimpleTokenTo(token);
        }

        private void AddSimpleTokenTo(ComplicatedToken token)
        {
            if (currentPossiblePhrase.Length > 0)
                token?.ChildTokens.Add(new SimpleToken(currentPossiblePhrase
                    .ToString()));
        }

        private string MakeHtmlFromMarkdown() => new TokenToHtmlConverter(AllTokens.Peek()).ConvertToHtml();
    }
}