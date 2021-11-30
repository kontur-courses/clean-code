using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class Tokenizer
    {
        private readonly Dictionary<string, MdWrapSetting> wrapSettings;
        private readonly List<Func<string, Token, bool>> ignoreTokenRules;
        private readonly List<Func<Token, IEnumerable<Token>, bool>> ignoreGroupTokenRules;
        private int[] mdTagSignatures;


        public Tokenizer()
        {
            var backslashSetting = new MdWrapSetting("\\", MdTagType.Backslash);
            wrapSettings = new Dictionary<string, MdWrapSetting>
            {
                {backslashSetting.MdTag, backslashSetting}
            };

            ignoreTokenRules = new List<Func<string, Token, bool>>();
            ignoreGroupTokenRules = new List<Func<Token, IEnumerable<Token>, bool>>();
        }

        public Tokenizer(IEnumerable<MdWrapSetting> wrapSettings)
            : this()
        {
            foreach (var setting in wrapSettings)
                this.wrapSettings.TryAdd(setting.MdTag, setting);
        }

        public Tokenizer(IEnumerable<MdWrapSetting> wrapSettings,
            IEnumerable<Func<string, Token, bool>> ignoreTokenRules,
            IEnumerable<Func<Token, IEnumerable<Token>, bool>> ignoreGroupTokenRules)
            : this(wrapSettings)
        {
            this.ignoreTokenRules.AddRange(ignoreTokenRules);
            this.ignoreGroupTokenRules.AddRange(ignoreGroupTokenRules);
        }

        public Token Tokenize(string text)
        {
            mdTagSignatures = wrapSettings
                .Select(ws => ws.Key.Length)
                .Distinct()
                .OrderByDescending(x => x)
                .ToArray();

            var root = new Token(text);
            foreach (var line in GetLines(text))
            {
                var tokens = GetTokens(line.Text).OrderBy(token => token.Position).ToList();
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
                yield return new Token(line, position, new MdWrapSetting("", MdTagType.Block));
                position += line.Length + Environment.NewLine.Length;
            }
        }
        
        private IEnumerable<Token> GetTokens(string text)
        {
            var spanTags = new List<Token>();

            Token lastBackslash = null;
            foreach (var tag in GetTags(text))
            {
                if (lastBackslash != null && tag.Position - lastBackslash.Position == 1)
                {
                    yield return text.GetToken(lastBackslash, tag);
                    continue;
                }

                if (tag.WrapSetting.IgnoreTagRules
                    .Select(rule => rule.Invoke(text, tag))
                    .Any(result => result))
                    continue;

                switch (tag.WrapSetting.TagType)
                {
                    case MdTagType.Backslash:
                        lastBackslash = tag;
                        break;
                    case MdTagType.Block:
                        yield return text.GetTokenUntilNewLine(tag);
                        break;
                    case MdTagType.Span:
                        if (TryBuildSpanToken(text, spanTags, tag, out var openTag))
                        {
                            yield return text.GetToken(openTag, tag);
                            spanTags.Remove(openTag);
                        }
                        else
                            spanTags.Add(tag);

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private IEnumerable<Token> GetTags(string text)
        {
            for (var pos = 0; pos < text.Length; pos++)
            {
                foreach (var mdTagSignature in mdTagSignatures)
                {
                    if (pos + mdTagSignature > text.Length ||
                        !wrapSettings.TryGetValue(text.Substring(pos, mdTagSignature), out var setting))
                        continue;

                    yield return new Token(pos, setting);
                    pos += setting.MdTag.Length - 1;
                    break;
                }
            }
        }

        private bool TryBuildSpanToken(string text, IEnumerable<Token> openTags, Token closeTag, out Token tag)
        {
            tag = null;

            foreach (var openTag in openTags.Where(openTag => openTag.WrapSetting == closeTag.WrapSetting))
            {
                tag = openTag;
                if (ignoreTokenRules
                    .Select(rule => rule.Invoke(text, text.GetToken(openTag, closeTag)))
                    .All(result => !result))
                    return true;
            }

            return false;
        }
    }
}