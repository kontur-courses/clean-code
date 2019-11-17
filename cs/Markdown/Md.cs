using Markdown.SpecialSymbols;
using Markdown.TagsDataBase;
using Markdown.TagsTokensReplacers;
using System;
using System.Linq;

namespace Markdown
{
    class Md
    {
        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            return EscapeSymbol.RemoveAllEscapeSymbolEntries(PairTagsRender(text));
        }

        private string PairTagsRender(string text)
        {
            var correctTagsPair = new MdTagsParsers.PairTagsParser().ParsePairTags(
                text,
                EscapeSymbol
                .FindAllPairsEscapeAndEscapedSymbols(text)
                .SelectMany(pair => new[] { pair.escapeSymbolIndex, pair.escapedSymbolsIndex }));
            return DefaultTagsTokensReplacer.ReplaceTagTokensInString(
                text,
                correctTagsPair.SelectMany(pair => new[] { pair.open, pair.close }),
                MdAndHtmlTags.GetHtmlTagByMdTag);
        }
    }
}