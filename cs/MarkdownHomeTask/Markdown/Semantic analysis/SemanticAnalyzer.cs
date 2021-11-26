using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class SemanticAnalyzer
    {
        private static readonly IReadOnlyDictionary<string, int> TagPriority =
            new Dictionary<string, int>
            {
                { "_", 1 },
                { "__", 2 }
            };

        private static readonly IReadOnlyCollection<string> SentenceModiferTags = new List<string>()
        {
            "#"
        };


        public IReadOnlyList<AnalyzedToken> Analise(IReadOnlyList<Token> sourceTokens)
        {
            List<AnalyzedToken> result = new();

            var opened = TagPriority
                .ToDictionary(dict => dict.Key, d => false);

            var openedTagsStack = new Stack<AnalyzedToken>();
            var isInsideWord = false;
            var hasWhiteSpace = false;

            for (var i = 0; i < sourceTokens.Count; i++)
            {
                if (sourceTokens[i].IsWhiteSpace)
                {
                    if (isInsideWord)
                    {
                        var lastTag = openedTagsStack.Pop().ToTextToken().Value;
                        opened[lastTag] = false;
                    }

                    isInsideWord = false;
                    hasWhiteSpace = true;
                }

                if (!sourceTokens[i].IsTag)
                {
                    result.Add(new AnalyzedToken(sourceTokens[i]));
                    continue;
                }

                if (SentenceModiferTags.Contains(sourceTokens[i].Value))
                {
                    result.Add(i == 0
                        ? new AnalyzedToken(sourceTokens[i], AnalyzedTokenType.SentenceModifer)
                        : new AnalyzedToken(sourceTokens[i].ToTextToken(), AnalyzedTokenType.SentenceModifer));
                    continue;
                }

                isInsideWord = IsInsideWord(sourceTokens, i);

                var analyzedToken = new AnalyzedToken(sourceTokens[i]);
                var lastTokenTag = result.Count > 0 && result[result.Count - 1].IsTag
                    ? result[result.Count - 1].Value
                    : "";

                result.Add(analyzedToken);

                var tag = analyzedToken.Value;

                if (!opened[tag])
                {
                    if (TryOpenTag(openedTagsStack, analyzedToken, opened, sourceTokens, i))
                        continue;
                }

                else if (openedTagsStack.Count > 0 && openedTagsStack.Peek().Value.Equals(tag))
                {
                    if (!(isInsideWord && hasWhiteSpace)
                        && !tag.Equals(lastTokenTag)
                        && TryCloseTag(sourceTokens, i, openedTagsStack, analyzedToken, opened))
                    {
                        isInsideWord = false;
                        hasWhiteSpace = false;
                        continue;
                    }
                }

                else
                {
                    PopAllTagsToText(openedTagsStack);
                }

                analyzedToken.ToTextToken();
            }

            PopAllTagsToText(openedTagsStack);

            return result;
        }


        private bool TryCloseTag(IReadOnlyList<Token> sourceTokens,
            int index,
            Stack<AnalyzedToken> openedTagsStack,
            AnalyzedToken analyzedToken,
            IDictionary<string, bool> opened)
        {
            if (!ClosingTagCheck(sourceTokens, index))
            {
                return false;
            }

            openedTagsStack.Pop().ChangeTagType(AnalyzedTokenType.Opener);
            analyzedToken.ChangeTagType(AnalyzedTokenType.Closing);
            opened[analyzedToken.Value] = false;

            return true;
        }

        private bool TryOpenTag(Stack<AnalyzedToken> openedTagsStack,
            AnalyzedToken analyzedToken,
            IDictionary<string, bool> opened,
            IReadOnlyList<Token> sourceTokens,
            int index)
        {
            var tag = analyzedToken.Value;

            if (!CheckTagPriority(openedTagsStack, tag) || !OpenedTagCheck(sourceTokens, index))
            {
                return false;
            }

            openedTagsStack.Push(analyzedToken);
            opened[tag] = true;

            return true;
        }

        private static bool CheckTagPriority(Stack<AnalyzedToken> openedTagsStack, string tag)
        {
            return !(openedTagsStack.Count > 0
                     && TagPriority[tag] > TagPriority[openedTagsStack.Peek().Value]);
        }

        private static void PopAllTagsToText(Stack<AnalyzedToken> openedTagsStack)
        {
            while (openedTagsStack.Count != 0)
            {
                openedTagsStack.Pop().ToTextToken();
            }
        }

        private bool OpenedTagCheck(IReadOnlyList<Token> sourceTokens, int index)
            => (index >= sourceTokens.Count - 1 || !sourceTokens[index + 1].IsWhiteSpace)
               && !HasBorderWithDigits(sourceTokens, index);

        private bool ClosingTagCheck(IReadOnlyList<Token> sourceTokens, int index)
            => (index == 0 || !sourceTokens[index - 1].IsWhiteSpace)
               && !HasBorderWithDigits(sourceTokens, index);

        private bool IsInsideWord(IReadOnlyList<Token> tokens, int index)
        {
            if (index == 0 || index == tokens.Count - 1)
            {
                return false;
            }

            return tokens[index - 1].IsText && tokens[index + 1].IsText;
        }

        private bool HasBorderWithDigits(IReadOnlyList<Token> tokens, int index)
        {
            var hasBorder = false;
            if (index > 0)
            {
                var val = tokens[index - 1].Value;
                hasBorder = char.IsDigit(val[val.Length - 1]);
            }

            if (index < tokens.Count - 1)
            {
                var val = tokens[index + 1].Value;
                hasBorder = hasBorder || char.IsDigit(val[0]);
            }

            return hasBorder;
        }
    }
}