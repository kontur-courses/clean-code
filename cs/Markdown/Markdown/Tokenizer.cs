using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly Dictionary<string, MdWrapSetting> wrapSettings = new Dictionary<string, MdWrapSetting>();
        private readonly List<Action<Token>> mdSpecificationRules = new List<Action<Token>>();
        private int[] mdTagSignatures;

        public Tokenizer()
        {
            var backslashSetting = new MdWrapSetting("\\", MdTagType.Backslash);
            wrapSettings.Add(backslashSetting.MdTag, backslashSetting);
        }

        public Tokenizer(IEnumerable<MdWrapSetting> wrapSettings)
            : this()
        {
            foreach (var setting in wrapSettings)
                this.wrapSettings.TryAdd(setting.MdTag, setting);
        }

        public Tokenizer(IEnumerable<MdWrapSetting> wrapSettings, IEnumerable<Action<Token>> mdSpecificationRules)
            : this(wrapSettings)
        {
            foreach (var mdSpecificationRule in mdSpecificationRules)
                this.mdSpecificationRules.Add(mdSpecificationRule);
        }

        public Token Tokenize(string text)
        {
            mdTagSignatures = wrapSettings
                .Select(ws => ws.Key.Length)
                .Distinct()
                .OrderByDescending(x => x)
                .ToArray();

            
            return Tokenize(new Token(text));
        }

        private Token Tokenize(Token root)
        {
            // if (root.WrapSetting.TagType == MdTagType.Backslash)
            //     return root;

            for (var pos = 0; pos < root.Text.Length; pos++)
            {
                if (!TryGetToken(root.Text, ref pos, out var childToken))
                    continue;
                root.AddToken(childToken);
                pos += childToken.WrapSetting.MdTag.Length - 1;
            }

            // foreach (var child in root.AllDescendants)
            //     foreach (var rule in mdSpecificationRules)
            //         rule.Invoke(child);
            
            return root;
        }

        private bool TryGetToken(string text, ref int openPos, out Token token)
        {
            token = null;

            if (!TryGetOpenMdTag(text, openPos, out var setting))
                return false;

            for (var pos = openPos + setting.MdTag.Length;
                pos <= (setting.TagType == MdTagType.Backslash ? openPos + 1 : text.Length);
                pos++)
            {
                var closeTagLength = TryGetCloseMdTag(text, ref pos, setting);
                if (closeTagLength == -1)
                    continue;

                token = new Token(text.Substring(openPos, pos - openPos + closeTagLength), openPos, setting);
                return true;
            }

            openPos += setting.MdTag.Length - 1;
            return false;
        }

        private bool TryGetOpenMdTag(string text, int pos, out MdWrapSetting setting)
        {
            setting = null;

            foreach (var mdTagSignature in mdTagSignatures)
            {
                if (pos + mdTagSignature <= text.Length &&
                    wrapSettings.TryGetValue(text.Substring(pos, mdTagSignature), out setting))
                    return true;
            }

            return false;
        }

        private int TryGetCloseMdTag(string text, ref int pos, MdWrapSetting setting)
        {
            switch (setting.TagType)
            {
                case MdTagType.Span:
                    if (TryGetOpenMdTag(text, pos, out var newSetting))
                        if (newSetting == setting)
                            return setting.MdCloseTag.Length;
                        else if (newSetting.TagType == MdTagType.Backslash &&
                                 TryGetOpenMdTag(text, pos + 1, out var backlashSetting))
                            pos += backlashSetting.MdTag.Length;
                        else
                            pos += newSetting.MdTag.Length;
                    else if (text.IsSubstring(pos, Environment.NewLine))
                        pos += text.Length - pos;
                    break;
                case MdTagType.Block:
                    if (text.IsSubstring(pos, setting.MdCloseTag))
                        return setting.MdCloseTag.Length;
                    else if (pos == text.Length)
                        return 0;
                    break;
                case MdTagType.Backslash:
                    if (TryGetOpenMdTag(text, pos, out setting))
                        return setting.MdTag.Length;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return -1;
        }
    }
}