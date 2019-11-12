using Markdown.MdTags;
using Markdown.MdTagsConverters;
using Markdown.TagsTokensReplacers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            return MdSpecialCharacters.RemoveAllEscapeCharacterEntries(PairTagsRender(text));
        }

        private string PairTagsRender(string text)
        {
            var correctTagsPair = new MdTagsParsers.PairTagsParser().ParsePairTags(text);
            return DefaultTagsTokensReplacer.ReplaceTagTokensInString(
                text,
                correctTagsPair.SelectMany(pair => new[] { pair.open, pair.close }),
                MdTagsToHtmlTagsConverter.GetHtmlTagByMdTag);
        }
    }
}