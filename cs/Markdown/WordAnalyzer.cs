using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Interfaces;
using Markdown.Morphemes;

namespace Markdown
{
    public class WordAnalyzer
    {
        private static readonly Dictionary<Tags, (IMorpheme Prefix, IMorpheme Postfix)> PairTags = new();

        private static readonly Tags[] SortedTags = { Tags.Italic, Tags.Bold, Tags.Header };

        private static readonly HashSet<char> SpecialSymbols = new()
        {
            '#',
            '_',
            '\\',
            '(',
            ')',
            '[',
            ']',
            '\n',
        };

        static WordAnalyzer()
        {
            var morphemes = new List<IMorpheme>()
            {
                new PostfixItalic(),
                new PrefixItalic(),
                new PostfixBold(),
                new PrefixBold(),
                new PrefixHeader(),
                new PostfixHeader()
            };

            var keys = morphemes.GroupBy(moprh => moprh.Tag)
                .Select(x => x.Key);

            var lookup = morphemes.ToLookup(m => m.Tag);
            foreach (var key in keys)
            {
                var ar = lookup[key].OrderBy(m => m.MorphemeType).ToArray();
                if (ar.Length != 2)
                    continue;
                PairTags[key] = (ar[0], ar[1]);
            }
        }

        //TODO refactoring
        public static List<ITag> AnalyzeWord(string word)
        {
            var result = new List<IMorpheme>();
            var prefixes = new Stack<IMorpheme>();
            var sb = new StringBuilder();
            var isShielding = false;

            for (var i = 0; i < word.Length; i++)
            {
                if (isShielding)
                {
                    isShielding = false;
                    if (SpecialSymbols.Contains(word[i]))
                    {
                        sb.Append(word[i]);
                        continue;
                    }

                    sb.Append('\\');
                }

                if (word[i] == '\\')
                {
                    isShielding = true;
                    continue;
                }

                if (!SpecialSymbols.Contains(word[i]))
                {
                    sb.Append(word[i]);
                    continue;
                }

                if (sb.Length != 0)
                    result.Add(new Word(sb.ToString()));
                sb.Clear();
                var morpheme = HighlightMorpheme(word, i, prefixes);

                if (morpheme.MorphemeType == MorphemeType.Prefix)
                    prefixes.Push(morpheme);

                result.Add(morpheme);
                i += morpheme.View.Length - 1;
            }

            if (sb.Length != 0)
                result.Add(new Word(sb.ToString()));

            return AnalyzeSequence(result);
        }


        private static bool CheckBoldTagInItalicTag(List<IMorpheme> word, Stack<int> prefixIndexes, int position)
        {
            return word[position].Tag == Tags.Bold && prefixIndexes.Any(index => word[index].Tag == Tags.Italic);
        }

        private static List<ITag> AnalyzeSequence(List<IMorpheme> word)
        {
            if (IsWordOnlyTag(word))
                return word.Select(m => (ITag)(new Word(m.View))).ToList();

            var prefixIndexes = new Stack<int>();
            var result = new List<ITag>();

            for (var i = 0; i < word.Count; i++)
            {
                if (i == word.Count - 1 && word[i].MorphemeType == MorphemeType.Postfix && prefixIndexes.Count == 0)
                {
                    result.Add(word[i]);
                    continue;
                }
                if (word[i].MorphemeType == MorphemeType.Postfix
                    && (prefixIndexes.Count == 0 || ReplaceTag(result, prefixIndexes, word[i].Tag))
                    || CheckBoldTagInItalicTag(word, prefixIndexes, i))
                {
                    result.Add(new Word(word[i].View));
                    continue;
                }
                if (word[i].MorphemeType == MorphemeType.Prefix)
                    prefixIndexes.Push(i);
                if (word[i].MorphemeType == MorphemeType.Postfix)
                    prefixIndexes.Pop();
                result.Add(word[i]);
            }
            BreakNoCloseTag(result, prefixIndexes);
            return result;
        }

        private static void BreakNoCloseTag(List<ITag> result, Stack<int> prefixIndexes)
        {
            while (prefixIndexes.Count != 0)
            {
                var index = prefixIndexes.Pop();
                if (index != 0)
                    result[index] = new Word(result[index].View);
            }
        }

        private static bool IsWordOnlyTag(List<IMorpheme> word)
        {
            if (word.Count == 1 && (word[0].TagType == TagType.Single || word[0].Tag == Tags.Header))
                return false;
            return word.All(w => w.MorphemeType != MorphemeType.Word);
        }

        private static bool ReplaceTag(List<ITag> result, Stack<int> prefixIndexes, Tags tag)
        {
            if (prefixIndexes.Count == 0 || result[prefixIndexes.Peek()].Tag == tag)
                return false;

            while (prefixIndexes.Count != 0)
            {
                var index = prefixIndexes.Pop();
                result[index] = new Word(result[index].View);
                if (result[index].Tag == tag)
                    break;
            }

            return true;
        }

        private static IMorpheme HighlightMorpheme(string context, int position, Stack<IMorpheme> prefixes)
        {
            //var  singleTag = CheckSingleTags(context, position);
            //if (singleTag.)
            foreach (var key in SortedTags)
            {
                var prefix = PairTags[key].Prefix;
                var postfix = PairTags[key].Postfix;
                var isPrefix = prefix.CheckForCompliance(context, position);
                var isPostfix = postfix.CheckForCompliance(context, position);
                if (isPostfix && isPrefix)
                {
                    if (prefixes.Count != 0 && prefixes.Peek().Tag == key)
                        return postfix;
                    return prefix;
                }

                if (isPrefix)
                    return prefix;

                if (isPostfix)
                    return postfix;
            }

            return new Word(context[position].ToString());
        }
    }
}