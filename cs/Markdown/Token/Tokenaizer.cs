using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly Dictionary<string, MdWrapSetting> wrapSettings = new Dictionary<string, MdWrapSetting>();

        public Tokenizer(params MdWrapSetting[] settings)
        {
            var defaultLineWrap = new MdWrapSetting("", "", MdWrapType.Paragraph);
            wrapSettings.TryAdd(defaultLineWrap.MdTag, defaultLineWrap);

            foreach (var setting in settings)
                wrapSettings.TryAdd(setting.MdTag, setting);
        }

        public IEnumerable<Token> Tokenize(string text)
        {
            foreach (var line in text.Split(Environment.NewLine))
            {
                foreach (var (mdTag, wrapSetting) in wrapSettings
                    .Where(x => x.Value.WrapType == MdWrapType.Paragraph)
                    .OrderByDescending(x => x.Key.Length))
                {
                    if (line.StartsWith(mdTag))
                    {
                        yield return new CompositeToken(line, 0, wrapSetting, ParseTextTokens(line));
                        break;
                    }   
                }
            }
        }

        private IEnumerable<Token> ParseTextTokens(string line)
        {
            foreach (var (mdTag, wrapSetting) in wrapSettings
                .Where(x => x.Value.WrapType == MdWrapType.Text)
                .OrderByDescending(x => x.Key.Length))
            {
                var startIndex = 0;
                while (true)
                {
                    var openTagPos = line.IndexOf(wrapSetting.MdTag, startIndex, StringComparison.Ordinal);
                    if (openTagPos == -1)
                        break;
                    var closeTagPos = line.IndexOf(wrapSetting.MdTag, openTagPos + 1, StringComparison.Ordinal);
                    if (closeTagPos == -1)
                        break;

                    yield return new Token(line[openTagPos..(startIndex = closeTagPos + wrapSetting.MdTag.Length)], openTagPos, wrapSetting);
                }
            }
        }
    }
}