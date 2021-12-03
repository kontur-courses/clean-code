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
        private readonly int[] mdTagSignatures;


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
            
            mdTagSignatures = this.parseTags
                .Select(ws => ws.Key.Length)
                .Distinct()
                .OrderByDescending(x => x)
                .ToArray();
        }

        public Tokenizer(IEnumerable<BaseMdTag> parseTags,
            IEnumerable<Func<Token, IEnumerable<Token>, bool>> ignoreGroupTokenRules)
            : this(parseTags)
        {
            this.ignoreGroupTokenRules.AddRange(ignoreGroupTokenRules);
        }

        
        public Token Tokenize(string text)
        {
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
            var tags = GetTags(text).ToList();
            var bsTokens = ParseBackslashTokens(text, tags).ToList();
            
            return ParseTokens(text, tags).Concat(bsTokens);
        }

        private IEnumerable<Tag> GetTags(string text)
        {
            for (var pos = 0; pos < text.Length; pos++)
            {
                if (!TryGetTag(text, pos, out var tag))
                    continue;

                yield return new Tag(pos, tag);
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

        private IEnumerable<Token> ParseBackslashTokens(string text, IList<Tag> tags)
        {
            for (var i = 0; i < tags.Count - 1; i++)
            {
                if (!(tags[i].MdTagType is BackslashMdTag)) 
                    continue;
                
                var pos = tags[i].Position;
                tags.Remove(tags[i]);

                if (tags[i].Position - pos == 1)
                {
                    yield return text.GetBackslashToken(tags[i]);
                    tags.Remove(tags[i]);
                }

                i--;
            }
        }

        private IEnumerable<Token> ParseTokens(string text, IList<Tag> tags)
        {
            for (var i = 0; i < tags.Count; i++)
            {
                if (!tags[i].MdTagType.TryGetToken(text, tags[i], tags, out var token, out var closeToken)) 
                    continue;

                if (closeToken != null)
                    tags.Remove(closeToken);
                
                yield return token;
                tags.RemoveAt(i);
                i--;
            }
        }
    }
}