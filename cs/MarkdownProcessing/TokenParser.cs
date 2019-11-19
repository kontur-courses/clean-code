using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownProcessing
{
    public class TokenParser
    {
        public Dictionary<string, TokenType> possibleTags = new Dictionary<string, TokenType>
        {
            {"_", TokenType.Italic},
            {"__", TokenType.Bold}
        };

        private string tagSymbols = "_";

        private readonly string input;
        private string tagToCheck;
        private StringBuilder currentPossibleTag;
        private StringBuilder currentPossiblePhrase;
        public readonly Stack<Token> AllTokens;

        public TokenParser(string input)
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
            char.IsLetter(symbol) || char.IsDigit(symbol) || char.IsWhiteSpace(symbol);

        private void CheckForOpeningTag()
        {
            tagToCheck = GetTagFromStringBuilder();
            if (possibleTags.ContainsKey(tagToCheck))
            {
                if (TagIsRepeating()) return;
                var parent = AllTokens.Peek() as ComplicatedToken;
                if (CurrentPhraseIsNotZero())
                    parent?.ChildTokens.Add(new SimpleToken(currentPossiblePhrase.ToString()));
                AllTokens.Push(new ComplicatedToken(possibleTags[tagToCheck]));
                RefreshTagAndPhraseBuffers();
                //currentPossiblePhrase = new StringBuilder();
            }
        }

        private string GetTagFromStringBuilder() =>
            currentPossibleTag.Length > 1
                ? currentPossibleTag.ToString().Substring(0, currentPossibleTag.Length - 1)
                : currentPossibleTag.ToString();

        private bool TagIsRepeating() => AllTokens.Peek().Type == possibleTags[tagToCheck];

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
            if (!possibleTags.ContainsKey(tagToCheck)) return;
            if (ClosingTagIsNotTheSame()) return;
            AddTokenToParentToken();
            RefreshTagAndPhraseBuffers();
        }

        private bool ClosingTagIsNotTheSame() => AllTokens.Peek().Type != possibleTags[tagToCheck];

        private void AddTokenToParentToken()
        {
            var token = AllTokens.Pop() as ComplicatedToken;
            var parentToken = AllTokens.Peek() as ComplicatedToken;
            parentToken?.ChildTokens.Add(token);
            AddSimpleTokenTo(token);
        }

        private void AddSimpleTokenTo(ComplicatedToken token)
        {
            token?.ChildTokens.Add(new SimpleToken(currentPossiblePhrase
                .Remove(currentPossiblePhrase.Length - tagToCheck.Length, tagToCheck.Length)
                .ToString()));
        }

        private string MakeHtmlFromMarkdown() => new MarkdownConverter(AllTokens.Peek()).ConvertToHtml();
    }
}