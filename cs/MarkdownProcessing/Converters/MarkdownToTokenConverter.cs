using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownProcessing.Markdowns;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Converters
{
    public class MarkdownToTokenConverter
    {
        private readonly string input;
        private readonly IResultMarkdown markdown;
        private string TagToCheck { get; set; }
        private StringBuilder CurrentPossibleTag { get; }
        private StringBuilder CurrentPossiblePhrase { get; }
        public Stack<Token> AllTokens { get; }

        public MarkdownToTokenConverter(string input, IResultMarkdown markdown)
        {
            this.input = input ?? throw new ArgumentException("Input was null");
            this.markdown = markdown;
            CurrentPossibleTag = new StringBuilder();
            CurrentPossiblePhrase = new StringBuilder();
            AllTokens = new Stack<Token>();
            AllTokens.Push(new ComplicatedToken(TokenType.Parent));
        }

        public string ParseInputIntoTokens()
        {
            var wasScreened = false;
            for (var index = 0; index < input.Length; index++)
            {
                var symbol = input[index];
                if (index > 0 && input[index - 1] == '\\')
                {
                    if (!wasScreened)
                    {
                        CurrentPossiblePhrase.Remove(CurrentPossiblePhrase.Length - 1, 1);
                        CurrentPossiblePhrase.Append(symbol);
                        wasScreened = true;
                        continue;
                    }

                    if (symbol == '\\' && wasScreened)
                    {
                        CurrentPossiblePhrase.Append(symbol);
                        wasScreened = false;
                        continue;
                    }
                }

                AppendSymbolToCurrentPhrases(symbol);
                if (!MarkdownTags.TagSymbols.Contains(symbol))
                    CurrentPossibleTag.Clear();
                CheckForOpeningTag();
                CheckForClosingTag();
                if (index != input.Length - 1 || CurrentPossiblePhrase.Length == 0)
                    continue;
                AddSimpleTokenTo(AllTokens.Peek() as ComplicatedToken);
                RefreshTagAndPhraseBuffers();
            }

            return MakeHtmlFromMarkdown();
        }

        private void AppendSymbolToCurrentPhrases(char symbol)
        {
            CurrentPossibleTag.Append(symbol);
            CurrentPossiblePhrase.Append(symbol);
        }

        private void CheckForOpeningTag()
        {
            TagToCheck = GetTagFromStringBuilder();
            if (!MarkdownTags.PossibleTags.ContainsKey(TagToCheck)) return;
            if (TagIsRepeating()) return;
            if (CurrentPhraseIsNotZero())
                AddSimpleTokenTo(AllTokens.Peek() as ComplicatedToken);
            AllTokens.Push(new ComplicatedToken(MarkdownTags.PossibleTags[TagToCheck]));
            RefreshTagAndPhraseBuffers();
        }

        private string GetTagFromStringBuilder()
        {
            return CurrentPossibleTag.Length > 1
                ? CurrentPossibleTag.ToString().Substring(0, CurrentPossibleTag.Length - 1)
                : CurrentPossibleTag.ToString();
        }

        private bool TagIsRepeating()
        {
            return AllTokens.Peek().Type == MarkdownTags.PossibleTags[TagToCheck];
        }

        private bool CurrentPhraseIsNotZero()
        {
            return CurrentPossiblePhrase.Remove(CurrentPossiblePhrase.Length - TagToCheck.Length, TagToCheck.Length)
                       .Length != 0;
        }

        private void RefreshTagAndPhraseBuffers()
        {
            CurrentPossibleTag.Clear();
            CurrentPossiblePhrase.Clear();
        }

        private void CheckForClosingTag()
        {
            if (AllTokens.Count == 1) return;
            TagToCheck = GetTagFromStringBuilder();
            if (!MarkdownTags.PossibleTags.ContainsKey(TagToCheck)) return;
            if (ClosingTagIsNotTheSame()) return;
            if (!CurrentPhraseIsNotZero())
            {
                var token = AllTokens.Pop();
                TagToCheck += CurrentPossibleTag;
                if (MarkdownTags.PossibleTags.ContainsKey(TagToCheck))
                    if (!ClosingTagIsNotTheSame())
                        AddTokenToParentToken();
                    else AllTokens.Push(new ComplicatedToken(MarkdownTags.PossibleTags[TagToCheck]));
                else AllTokens.Push(token);
            }
            else
            {
                AddTokenToParentToken();
            }

            RefreshTagAndPhraseBuffers();
        }

        private bool ClosingTagIsNotTheSame()
        {
            return AllTokens.Peek().Type != MarkdownTags.PossibleTags[TagToCheck];
        }

        private void AddTokenToParentToken()
        {
            var token = AllTokens.Pop() as ComplicatedToken;
            var parentToken = AllTokens.Peek() as ComplicatedToken;
            parentToken?.ChildTokens.Add(token);
            AddSimpleTokenTo(token);
        }

        private void AddSimpleTokenTo(ComplicatedToken token)
        {
            if (CurrentPossiblePhrase.Length > 0)
                token?.ChildTokens.Add(new SimpleToken(CurrentPossiblePhrase
                    .ToString()));
        }

        private string MakeHtmlFromMarkdown()
        {
            return new TokenToNewMarkdownConverter(AllTokens.Peek(), markdown).ConvertToHtml();
        }
    }
}