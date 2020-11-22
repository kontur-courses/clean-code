using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class Tokenizer
    {
        private static readonly char[] OpenTagChars = {'_', '['};
        private const char EscapeChar = '\\';

        public static IEnumerable<IToken> ParseIntoTokens(string mdText)
        {
            if (mdText.StartsWith("# "))
            {
                yield return HeaderToken.Create(mdText);
                yield break;
            }

            foreach (var token in GetAllAnotherTokens(mdText))
                yield return token;
        }

        private static IEnumerable<IToken> GetAllAnotherTokens(string mdText)
        {
            var isEscapedTag = false;
            var collector = new StringBuilder();
            for (var i = 0; i < mdText.Length; i++)
            {
                if (!isEscapedTag && CanBeEscaping(mdText, i))
                {
                    isEscapedTag = true;
                    continue;
                }

                if (isEscapedTag || IsNotTag(mdText, i))
                {
                    collector.Append(mdText[i]);
                    isEscapedTag = false;
                    continue;
                }

                if (TryGetToken(mdText, i, out var newToken))
                {
                    if (collector.Length != 0)
                    {
                        yield return StringToken.Create(collector.ToString());
                        collector.Clear();
                    }

                    yield return newToken;
                    i += newToken.MdTokenLength - 1;
                }
                else
                {
                    collector.Append(mdText[i]);
                }
            }

            if (collector.Length != 0)
                yield return StringToken.Create(collector.ToString());
        }

        private static bool TryGetToken(string mdText, in int index, out IToken token)
        {
            token = mdText[index] switch
            {
                '_' when mdText.HasUnderscoreAt(index + 1) => BoldToken.Create(mdText, index),
                '_' => ItalicToken.Create(mdText, index),
                '[' => LinkToken.Create(mdText, index),
                _ => default
            };
            return token is not null;
        }

        private static bool CanBeEscaping(string mdText, int i)
        {
            var canNextCharBeEscaped = mdText.IsCharInsideString(i + 1) &&
                                       (OpenTagChars.Contains(mdText[i + 1]) || mdText[i + 1] is EscapeChar);
            return mdText[i] is EscapeChar && canNextCharBeEscaped;
        }

        private static bool IsNotTag(string mdText, int i) =>
            !(OpenTagChars.Contains(mdText[i]) && !mdText.HasUnderscoreAt(i - 1) && mdText.HasNonWhiteSpaceAt(i + 1) &&
              mdText.HasNonWhiteSpaceAt(i + 2));
    }
}