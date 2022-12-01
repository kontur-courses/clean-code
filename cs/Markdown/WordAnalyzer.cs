using System.Collections.Generic;
using System.Text;
using Markdown.Interfaces;
using Markdown.LinkTags;
using Markdown.PairTags;

namespace Markdown
{
    public class WordAnalyzer
    {
        private static readonly Dictionary<Tag, (IPairTag Opening, IPairTag Closing)> PairTags = new()
        {
            [Tag.Italic] = (new OpeningItalic(), new ClosingItalic()),
            [Tag.Bold] = (new OpeningBold(), new ClosingBold()),
            [Tag.Header] = (new OpeningHeader(), new ClosingHeader())
        };

        private static readonly ILinkTag[] SingleTags = { new Image(), new Link() };

        private static readonly HashSet<char> SpecialSymbols = new() { '#', '_', '(', ')', '[', ']', '!' };

        //TODO refactoring
        public static List<ITag> SplitWordIntoTags(string word)
        {
            var result = new List<ITag>();
            var lastOpenTag = (ITag)null;

            for (var i = 0; i < word.Length;)
            {
                var tag = SpecialSymbols.Contains(word[i])
                    ? DefineMorpheme(word, i, lastOpenTag)
                    : DefineWord(word, i);

                if (tag.tag.TagType == TagType.Open)
                    lastOpenTag = tag.tag;

                result.Add(tag.tag);
                i = tag.EndIndex;
            }

            return AnalyzerSequence.AnalyzeWord(result);
        }


        private static (ITag tag, int EndIndex) DefineMorpheme(string context, int position, ITag lastPrefix)
        {
            var singleTag = CheckSingleTags(context, position);
            if (singleTag != null)
                return (singleTag, position + singleTag.ViewTag.Length);

            foreach (var key in PairTags.Keys)
            {
                var prefix = PairTags[key].Opening;
                var postfix = PairTags[key].Closing;
                var isPrefix = prefix.CheckForCompliance(context, position);
                var isPostfix = postfix.CheckForCompliance(context, position);
                if (isPostfix && isPrefix)
                {
                    if (lastPrefix != null && lastPrefix.Tag == key)
                        return (postfix, position + postfix.ViewTag.Length);
                    return (prefix, position + prefix.ViewTag.Length);
                }

                if (isPrefix)
                    return (prefix, position + prefix.ViewTag.Length);

                if (isPostfix)
                    return (postfix, position + postfix.ViewTag.Length);
            }

            return (new Word(context[position].ToString()), position + 1);
        }

        private static ITag CheckSingleTags(string context, int position)
        {
            foreach (var tag in SingleTags)
                if (tag.TryParse(context, position, out var a))
                    return a;
            return null;
        }

        private static (ITag tag, int EndIndex) DefineWord(string context, int position)
        {
            var sb = new StringBuilder();
            var isShielding = false;
            for (var i = position; i < context.Length; i++)
            {
                if (isShielding)
                {
                    isShielding = false;
                    if (SpecialSymbols.Contains(context[i]) || context[i] == '\\')
                    {
                        sb.Append(context[i]);
                        continue;
                    }

                    sb.Append('\\');
                }

                if (context[i] == '\\')
                {
                    isShielding = true;
                    continue;
                }

                if (!SpecialSymbols.Contains(context[i]))
                {
                    sb.Append(context[i]);
                    continue;
                }

                return (new Word(sb.ToString()), i);
            }

            return (new Word(sb.ToString()), context.Length);
        }
    }
}