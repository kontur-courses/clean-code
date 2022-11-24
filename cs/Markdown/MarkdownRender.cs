using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Interfaces;
using Markdown.Morphemes;

namespace Markdown
{
    public class MarkdownRender
    {
        public static string Render(ITranslatorFromMarkdown render, string text)
        {
            var tags = new List<ITag>();
            foreach (var paragraph in text.Split("\n\r\n\r", StringSplitOptions.RemoveEmptyEntries))
            {
                tags.AddRange(RenderParagraph(paragraph));
            }

            return render.Translate(tags);
        }

        private static List<ITag> RenderParagraph(string paragraph)
        {
            var result = new List<ITag>();
            foreach (var word in paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                result.AddRange(WordAnalyzer.AnalyzeWord(word));
                result.Add(new Word(" "));
            }

            result.RemoveAt(result.Count - 1);
            return AnalyzeParagraph(result);
        }

        //потенциально n^2, но я не представляю ситуацию, где это было действительно n^2
        private static bool CheckBoldTagInItalicTag(List<ITag> sequence, Stack<int> prefixIndexes, int position)
        {
            return sequence[position].Tag == Tags.Bold && prefixIndexes.Any(index => sequence[index].Tag == Tags.Italic);
        }

        private static bool CheckHeaderCorrectPosition(ITag tag, int position)
        {
            if (tag.Tag != Tags.Header)
                return false;
            return position == 0 && tag.TagType == TagType.Open 
                || tag.TagType == TagType.Close;
        }

        private static List<ITag> AnalyzeParagraph(List<ITag> sequence)
        {
            var prefixIndexes = new Stack<int>();
            var result = new List<ITag>();
            if(sequence[0].Tag == Tags.Header)
                sequence.Add(new PostfixHeader());
            
            for (var i = 0; i < sequence.Count; i++)
            {
                if (CheckBoldTagInItalicTag(sequence, prefixIndexes, i)
                    || sequence[i].Tag == Tags.Header && !CheckHeaderCorrectPosition(sequence[i], i)
                    || sequence[i].TagType == TagType.Close
                        && (prefixIndexes.Count == 0 || ReplaceTag(result, prefixIndexes, sequence[i].Tag)))
                {
                    result.Add(new Word(sequence[i].View));
                    continue;
                }
                if (sequence[i].TagType == TagType.Open)
                    prefixIndexes.Push(i);
                if (sequence[i].TagType == TagType.Close)
                    prefixIndexes.Pop();
                result.Add(sequence[i]);
            }

            BreakNoCloseTag(result, prefixIndexes);
            return result;
        }

        private static void BreakNoCloseTag(List<ITag> result, Stack<int> prefixIndexes)
        {
            while (prefixIndexes.Count != 0)
            {
                var index = prefixIndexes.Pop();
                result[index] = new Word(result[index].View);
            }
        }

        private static bool ReplaceTag(List<ITag> result, Stack<int> prefixIndexes, Tags tag)
        {
            if (prefixIndexes.Count == 0 || result[prefixIndexes.Peek()].Tag == tag)
                return false;
            while (prefixIndexes.Count != 0)
            {
                var index = prefixIndexes.Pop();
                if (result[index].Tag == Tags.Header)
                {
                    prefixIndexes.Push(index);
                    break;
                }
                result[index] = new Word(result[index].View);
                if (result[index].Tag == tag)
                    break;
            }

            return true;
        }
    }
}