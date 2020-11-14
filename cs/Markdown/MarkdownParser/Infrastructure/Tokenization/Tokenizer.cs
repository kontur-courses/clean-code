﻿using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Helpers;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public class Tokenizer
    {
        private readonly ITokenBuilder[] tokenBuilders;

        public Tokenizer(IEnumerable<ITokenBuilder> tokenBuilders)
        {
            this.tokenBuilders = tokenBuilders.ToArray();
        }

        public ParagraphData[] Tokenize(string rawInput) => rawInput
            .Split(Environment.NewLine)
            .WhereNot(string.IsNullOrEmpty)
            .Select(p => TokenizationWorker.CreateForParagraph(p, tokenBuilders))
            .Select(ParagraphData.FromWorker)
            .ToArray();

        public static ICollection<Token> MergeTextTokens(IEnumerable<Token> tokens)
        {
            var result = new List<Token>();
            var previousText = new List<TextToken>();
            foreach (var token in tokens)
            {
                if (token is TextToken textToken)
                {
                    previousText.Add(textToken);
                    continue;
                }

                if (previousText.Count != 0)
                {
                    result.Add(CreateMergedToken());
                    previousText.Clear();
                }

                result.Add(token);
            }

            if (previousText.Count != 0)
                result.Add(CreateMergedToken());

            return result;

            TextToken CreateMergedToken()
            {
                var text = string.Join(string.Empty, previousText.Select(x => x.RawValue));
                return new TextToken(previousText.First().StartPosition, text);
            }
        }
    }
}