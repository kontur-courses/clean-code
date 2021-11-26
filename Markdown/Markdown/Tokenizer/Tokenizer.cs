using System.Collections.Generic;
using System.Linq;
using Markdown.Factories;
using Markdown.Tokens;

namespace Markdown.Tokenizer
{
    public class Tokenizer : ITokenizer<MarkdownToken>
    {
        private static readonly HashSet<string> MarkdownTags = new() {"_", "__", "#"};
        private static readonly HashSet<string> PairedMarkdownTags = new() {"_", "__"};
        private static readonly HashSet<string> SingleMarkdownTags = new() {"# ", "#"};

        private readonly ITokenFactory<MarkdownToken> tokenFactory;

        public Tokenizer(ITokenFactory<MarkdownToken> tokenFactory)
        {
            this.tokenFactory = tokenFactory;
        }

        public IEnumerable<MarkdownToken> Tokenize(IEnumerable<string> rawTokens)
        {
            var preparedTokens = PrepareTokens(rawTokens);

            var tokens = PreTokenize(preparedTokens);

            ResolvePairedTags(tokens);

            ResolveSingleTags(tokens);

            ResolveDoubleUnderlineInSingleUnderline(tokens);

            return tokens;
        }

        private void ResolveSingleTags(List<MarkdownToken> tokens)
        {
            var isTagOpened = false;

            for (var i = 0; i < tokens.Count; ++i)
            {
                var token = tokens[i];
                if (token.Type == TokenType.SingleTag)
                {
                    if (isTagOpened)
                    {
                        tokens[i] = tokenFactory.NewToken(TokenType.Word, token.Value);
                    }
                    else
                    {
                        tokens[i] = tokenFactory.NewToken(TokenType.PairedTagOpened, token.Value);
                        isTagOpened = true;
                    }
                }

                if (token.Value == "\n")
                {
                    tokens[i] = tokenFactory.NewToken(TokenType.PairedTagClosed, token.Value);
                    isTagOpened = false;
                }
            }

            if (isTagOpened)
            {
                tokens.Add(tokenFactory.NewToken(TokenType.PairedTagClosed, ""));
            }
        }

        private void ResolveDoubleUnderlineInSingleUnderline(List<MarkdownToken> tokens)
        {
            var isSingleUnderlineOpened = false;

            for (var i = 0; i < tokens.Count; ++i)
            {
                var token = tokens[i];
                if (token.Type is TokenType.PairedTagOpened or TokenType.PairedTagClosed &&
                    token.Value == "_")
                {
                    isSingleUnderlineOpened = !isSingleUnderlineOpened;
                }

                if (isSingleUnderlineOpened && token.Value == "__")
                {
                    tokens[i] = tokenFactory.NewToken(TokenType.Word, token.Value);
                }
            }
        }

        private void ResolvePairedTags(List<MarkdownToken> tokens)
        {
            var stack = new Stack<(string token, int index)>();

            for (var i = 0; i < tokens.Count; ++i)
            {
                var token = tokens[i];
                if (token.Type is not TokenType.PairedTagOpened) continue;

                if (stack.Count > 0 && stack.Peek().token == token.Value)
                {
                    if (string.IsNullOrWhiteSpace(tokens[i - 1].Value))
                    {
                        tokens[i] = tokenFactory.NewToken(TokenType.Word, token.Value);
                    }
                    else
                    {
                        var pair = stack.Pop();
                        tokens[pair.index] = tokenFactory.NewToken(TokenType.PairedTagOpened, pair.token);
                        tokens[i] = tokenFactory.NewToken(TokenType.PairedTagClosed, token.Value);
                    }
                }
                else if (stack.Count < PairedMarkdownTags.Count)
                {
                    if (i + 1 < tokens.Count && string.IsNullOrWhiteSpace(tokens[i + 1].Value))
                    {
                        tokens[i] = tokenFactory.NewToken(TokenType.Word, token.Value);
                    }
                    else
                    {
                        stack.Push((token.Value, i));
                    }
                }
                else if (stack.Count == PairedMarkdownTags.Count)
                {
                    while (stack.Count > 0)
                    {
                        var pair = stack.Pop();
                        tokens[pair.index] = tokenFactory.NewToken(TokenType.Word, pair.token);
                    }

                    tokens[i] = tokenFactory.NewToken(TokenType.Word, token.Value);
                }
            }

            while (stack.Count > 0)
            {
                var pair = stack.Pop();
                tokens[pair.index] = tokenFactory.NewToken(TokenType.Word, pair.token);
            }
        }

        private List<MarkdownToken> PreTokenize(IEnumerable<string> preparedTokens)
        {
            var tokens = new List<MarkdownToken>();

            foreach (var token in preparedTokens)
            {
                if (PairedMarkdownTags.Contains(token))
                {
                    tokens.Add(tokenFactory.NewToken(TokenType.PairedTagOpened, token));
                }
                else if (SingleMarkdownTags.Contains(token))
                {
                    tokens.Add(tokenFactory.NewToken(TokenType.SingleTag, token));
                }
                else
                {
                    tokens.Add(tokenFactory.NewToken(TokenType.Word, token));
                }
            }

            return tokens;
        }

        private static IEnumerable<string> PrepareTokens(IEnumerable<string> rawTokens)
        {
            var preparedTokens = MergeRawTokens(rawTokens.ToList(), "\\");

            preparedTokens = MergeRawTokens(preparedTokens, "_");

            preparedTokens = MergeRawTokens(preparedTokens, ("#", " "));

            preparedTokens = MergeRawTokens(preparedTokens, "__");

            preparedTokens = MergeEscapeCharactersToTags(preparedTokens);

            return preparedTokens;
        }

        private static List<string> MergeEscapeCharactersToTags(List<string> rawTokens)
        {
            var result = rawTokens;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var tag in MarkdownTags)
            {
                result = MergeRawTokens(result, ("\\", tag));
            }

            return result;
        }

        private static List<string> MergeRawTokens(List<string> rawTokens, string tokensToMerge)
        {
            return MergeRawTokens(rawTokens, (tokensToMerge, tokensToMerge));
        }

        private static List<string> MergeRawTokens(List<string> rawTokens,
            (string firstToken, string secondToken) tokensToMerge)
        {
            var mergedTokens = new List<string>();

            var merged = tokensToMerge.firstToken + tokensToMerge.secondToken;
            var index = 0;

            while (index < rawTokens.Count)
            {
                var currentToken = rawTokens[index];

                if (index < rawTokens.Count - 1 &&
                    currentToken == tokensToMerge.firstToken &&
                    rawTokens[index + 1] == tokensToMerge.secondToken)
                {
                    currentToken = merged;
                    index++;
                }

                mergedTokens.Add(currentToken);
                index++;
            }

            return mergedTokens;
        }
    }
}