using System.Collections.Generic;
using System.Linq;
using Markdown.Interfaces;

namespace Markdown
{
    public class AnalyzerSequence
    {
        public static List<ITag> AnalyzeWord(List<ITag> word)
        {
            if (IsHeader(word))
                return word;

            if (IsSequenceConsistsOnlyTags(word))
                return word.Select(m => (ITag)new Word(m.ViewTag)).ToList();

            if (IsTagsOnlyBeginning(word) || IsTagsOnlyEnd(word))
                return word;

            return AnalyzeSequence(word);
        }

        private static bool IsHeader(IReadOnlyList<ITag> word)
        {
            return word.Count == 1 && word[0].Tag == Tag.Header;
        }

        private static bool IsTagsOnlyEnd(IReadOnlyList<ITag> word)
        {
            var open = new Stack<int>();
            var close = new Stack<int>();
            for (var i = 0; i < word.Count; i++)
            {
                if (word[i].TagType == TagType.Open)
                    open.Push(i);
                if (word[i].TagType != TagType.Close) continue;

                if (open.Count != 0 && word[i].Tag != word[open.Peek()].Tag)
                    return false;
                if (open.Count != 0)
                    open.Pop();
                close.Push(i);
            }

            if (open.Count != 0)
                return false;
            var set = new HashSet<int>(Enumerable.Range(word.Count - close.Count, close.Count));

            return close.Select(i => set.Contains(i)).All(b => b);
        }

        private static bool IsTagsOnlyBeginning(IReadOnlyList<ITag> word)
        {
            var open = new Stack<int>();
            for (var i = 0; i < word.Count; i++)
            {
                if (word[i].TagType == TagType.Open)
                    open.Push(i);
                if (word[i].TagType == TagType.Close)
                {
                    if (open.Count == 0 || word[i].Tag != word[open.Peek()].Tag)
                        return false;
                    open.Pop();
                }
            }

            if (open.Count == 0)
                return true;
            var set = new HashSet<int>(Enumerable.Range(0, open.Count));

            return open.Select(i => set.Contains(i)).All(b => b);
        }

        public static List<ITag> AnalyzeSequence(IReadOnlyList<ITag> sequence)
        {
            var openTagIndexes = new Stack<int>();
            var result = new List<ITag>();

            for (var i = 0; i < sequence.Count; i++)
            {
                if ((sequence[i].TagType == TagType.Close && !IsTagHasBeenOpened(sequence, sequence[i], openTagIndexes))
                    || IsBoldTagInItalicTag(sequence, openTagIndexes, i)
                    || (sequence[i].TagType == TagType.Open && sequence[i].Tag == Tag.Header && i != 0))
                {
                    result.Add(new Word(sequence[i].ViewTag));
                    if (!IsBoldTagInItalicTag(sequence, openTagIndexes, i) &&
                        !IsTagHasBeenOpened(sequence, sequence[i], openTagIndexes))
                        BreakNoCloseTag(result, openTagIndexes);
                    continue;
                }

                if (sequence[i].TagType == TagType.Open)
                    openTagIndexes.Push(i);
                if (sequence[i].TagType == TagType.Close)
                    openTagIndexes.Pop();
                result.Add(sequence[i]);
            }

            BreakNoCloseTag(result, openTagIndexes);
            return result;
        }

        private static bool IsSequenceConsistsOnlyTags(IReadOnlyList<ITag> word)
        {
            if (word.Count == 1 && word[0].TagType == TagType.Single)
                return false;
            return word.All(w => w.TagType != TagType.None);
        }

        private static bool IsBoldTagInItalicTag(IReadOnlyList<ITag> word, IEnumerable<int> prefixIndexes, int position)
        {
            return word[position].Tag == Tag.Bold && prefixIndexes.Any(index => word[index].Tag == Tag.Italic);
        }

        private static bool IsTagHasBeenOpened(IReadOnlyList<ITag> sequence, ITag tag, Stack<int> openTagIndexes)
        {
            return !(openTagIndexes.Count == 0 || sequence[openTagIndexes.Peek()].Tag != tag.Tag);
        }

        private static void BreakNoCloseTag(IList<ITag> sequence, Stack<int> prefixIndexes)
        {
            while (prefixIndexes.Count != 0)
            {
                var index = prefixIndexes.Pop();
                sequence[index] = new Word(sequence[index].ViewTag);
            }
        }
    }
}