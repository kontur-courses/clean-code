using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class Tokenizer
    {
        private readonly Dictionary<string, BaseMdTag> parseTags;
        private readonly List<Func<Token, IEnumerable<Token>, bool>> ignoreGroupTokenRules;
        private int[] mdTagSignatures;


        public Tokenizer()
        {
            var bsTag = new BackslashMdTag();
            parseTags = new Dictionary<string, BaseMdTag>
            {
                {bsTag.MdTag, bsTag}
            };

            ignoreGroupTokenRules = new List<Func<Token, IEnumerable<Token>, bool>>();
        }

        public Tokenizer(IEnumerable<BaseMdTag> parseTags)
            : this()
        {
            foreach (var parseTag in parseTags)
                this.parseTags[parseTag.MdTag] = parseTag;
        }

        public Tokenizer(IEnumerable<BaseMdTag> parseTags,
            IEnumerable<Func<Token, IEnumerable<Token>, bool>> ignoreGroupTokenRules)
            : this(parseTags)
        {
            this.ignoreGroupTokenRules.AddRange(ignoreGroupTokenRules);
        }


        public Token Tokenize(string text)
        {
            mdTagSignatures = parseTags
                .Select(ws => ws.Key.Length)
                .Distinct()
                .OrderByDescending(x => x)
                .ToArray();

            var root = new Token(text);
            foreach (var line in GetLines(text))
            {
                var tokens = GetTokens(line.Value).OrderBy(token => token.Position).ToList();
                foreach (var token in tokens
                    .Where(token => ignoreGroupTokenRules
                        .Select(rule => rule.Invoke(token, tokens))
                        .All(result => !result)))
                    line.AddToken(token);
                root.AddToken(line);
            }

            return root;
        }

        private static IEnumerable<Token> GetLines(string text)
        {
            var position = 0;
            foreach (var line in text.Split(Environment.NewLine))
            {
                yield return new Token(line, position, new BlockMdTag());
                position += line.Length + Environment.NewLine.Length;
            }
        }

        private IEnumerable<Token> GetTokens(string text)
        {
            var usedTagsAtPositions = new Dictionary<int, int>();
            for (var pos = 0; pos < text.Length; pos++)
            {
                if (usedTagsAtPositions.TryGetValue(pos, out var skip))
                {
                    pos += skip - 1;
                    continue;
                }

                if (!TryGetTag(text, pos, out var tag))
                    continue;

                if (tag is BackslashMdTag backslash &&
                    TryGetTag(text, pos + 1, out var backSlashedTag))
                {
                    yield return text.GetToken(pos, pos + backslash.Length + backSlashedTag.Length, backslash);
                    pos += backslash.Length + backSlashedTag.Length - 1;
                    continue;
                }
                
                if (!tag.IsTag(text, pos))
                    continue;

                if (!tag.TryGetToken(text, pos, out var token))
                    continue;

                if (tag.HasCloseMdTag)
                    usedTagsAtPositions.Add(pos + token.Value.Length - tag.Length, tag.Length);
                yield return token;
                pos += tag.Length - 1;
            }
        }

        private bool TryGetTag(string text, int pos, out BaseMdTag mdTag)
        {
            foreach (var mdTagSignature in mdTagSignatures)
            {
                if (pos + mdTagSignature > text.Length ||
                    !parseTags.TryGetValue(text.Substring(pos, mdTagSignature), out var tag))
                    continue;

                mdTag = tag;
                return true;
            }

            mdTag = null;
            return false;
        }
    }
}