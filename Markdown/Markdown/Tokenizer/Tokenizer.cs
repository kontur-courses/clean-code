using System.Collections.Generic;
using System.Linq;
using Markdown.Factories;
using Markdown.Tokens;

namespace Markdown.Tokenizer
{
    public class Tokenizer<T> : ITokenizer<T>
        where T : IToken
    {
        private readonly ITokenFactory<T> tokenFactory;

        public Tokenizer(ITokenFactory<T> tokenFactory)
        {
            this.tokenFactory = tokenFactory;
        }

        public IEnumerable<T> Tokenize(IEnumerable<string> rawTokens)
        {
            var preparedTokens = PrepareTokens(rawTokens);

            return preparedTokens.Select(token => Tags.MarkdownTags.Contains(token)
                    ? tokenFactory.NewToken(TokenType.Tag, token)
                    : tokenFactory.NewToken(TokenType.Word, token))
                .ToList();
        }

        private static IEnumerable<string> PrepareTokens(IEnumerable<string> rawTokens)
        {
            var preparedTokens = MergeRawTokens(rawTokens, "_");

            preparedTokens = MergeRawTokens(preparedTokens, "\\");

            preparedTokens = MergeEscapeCharactersToTags(preparedTokens);

            return preparedTokens;
        }

        private static IEnumerable<string> MergeRawTokens(IEnumerable<string> rawTokens, string tokensToMerge)
        {
            return MergeRawTokens(rawTokens, (tokensToMerge, tokensToMerge));
        }

        private static IEnumerable<string> MergeRawTokens(IEnumerable<string> rawTokens,
            (string firstToken, string secondToken) tokensToMerge)
        {
            var mergedTokens = new List<string>();

            var merged = tokensToMerge.firstToken + tokensToMerge.secondToken;
            // ReSharper disable once ConvertToUsingDeclaration
            using (var enumerator = rawTokens.GetEnumerator())
            {
                do
                {
                    var currentToken = enumerator.Current;

                    if (currentToken == tokensToMerge.firstToken &&
                        enumerator.MoveNext() &&
                        enumerator.Current == tokensToMerge.secondToken)
                    {
                        currentToken = merged;
                    }
                    else
                    {
                        mergedTokens.Add(currentToken);
                        mergedTokens.Add(enumerator.Current);
                    }

                    mergedTokens.Add(currentToken);
                } while (enumerator.MoveNext());
            }

            return mergedTokens;
        }

        private static IEnumerable<string> MergeEscapeCharactersToTags(IEnumerable<string> rawTokens)
        {
            var mergedTags = rawTokens;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var tag in Tags.MarkdownTags)
            {
                mergedTags = MergeRawTokens(mergedTags, ("\\", tag));
            }

            return mergedTags;
        }
    }
}