using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;
using Markdown.Tag.Standart;

namespace Markdown.Translator
{

    public class PretenderInfo
    {
        public MarkdownTag Tag;
        public int Position;

        public PretenderInfo(MarkdownTag tag, int position)
        {
            this.Tag = tag;
            this.Position = position;
        }
    }

    public class MarkdownTranslator : IMarkdownTranslator
    {
        private readonly IReadOnlyCollection<MarkdownTag> tagsCollection;
        private readonly Stack<MarkdownTag> tagsNesting;
        private readonly Stack<PretenderInfo> pretenders;
        private int pointer;

        public MarkdownTranslator()
        {
            tagsCollection = new List<MarkdownTag>
            {
                new Italic(),
                new Bold()
            };
            pretenders = new Stack<PretenderInfo>();
            tagsNesting = new Stack<MarkdownTag>();
        }

        public MarkdownTranslator(IReadOnlyCollection<MarkdownTag> tagsCollection)
        {
            this.tagsCollection = tagsCollection;
            tagsNesting = new Stack<MarkdownTag>();
            pretenders = new Stack<PretenderInfo>();
        }

        public string Translate(string text)
        {
            var delta = 0;
            pointer = 0;
            var result = new StringBuilder();
            for (; pointer < text.Length; pointer++)
            {
                var currentTag = GetTag(text);
                if (currentTag != default(MarkdownTag))
                {
                    if (pretenders.Any())
                    {
                        var previousPretender = pretenders.Peek();
                        if (currentTag == previousPretender.Tag)
                        {
                            if (!IsCorrectTagEnding(currentTag.Tag, text))
                            {
                                result.Append(currentTag.Tag);
                                continue;
                            }

                            var pretender = pretenders.Pop();
                            var length = result.Length;
                            result.Remove(pretender.Position + delta, pretender.Tag.Length);
                            result.Insert(pretender.Position + delta, pretender.Tag.OpenTagTranslation);
                            result.Append(pretender.Tag.CloseTagTranslation);
                            delta += result.Length - 1 - length;
                            foreach (var pretenderInfo in pretenders)
                                pretenderInfo.Position -= delta;
                        }
                        else if (previousPretender.Tag.CanContain(currentTag))
                        {
                            pretenders.Push(new PretenderInfo(currentTag, pointer));
                            pointer += currentTag.Length - 1;
                            result.Append(currentTag.Tag);
                        }
                    }
                    else
                    {
                        pretenders.Push(new PretenderInfo(currentTag, pointer));
                        pointer += currentTag.Length - 1;
                        result.Append(currentTag.Tag);
                    }
                }
                else
                    result.Append(text[pointer]);
            }

            return result.ToString();
        }

        private MarkdownTag GetTag(string text)
        {
            var possibleTags = tagsCollection
                .Where(tag => IsCorrectTagOpening(tag.Tag, text)
                              || IsCorrectTagEnding(tag.Tag, text));
            return possibleTags.FirstOrDefault();
        }

        private bool IsCorrectTagOpening(string tag, string line)
        {
            if (pointer + tag.Length >= line.Length)
                return false;
            if (pointer != 0 && line[pointer - 1] == '\\')
                return false;

            var nextChar = line[pointer + tag.Length];
            return
                IsSameTag(line, tag) &&
                char.IsLetter(nextChar);
        }

        private bool IsSameTag(string line, string tag)
        {
            if (pointer + tag.Length > line.Length)
                return false;
            for (var i = pointer; i < pointer + tag.Length; i++)
            {
                if (line[i] != tag[i - pointer])
                    return false;
            }

            return true;
        }

        private bool IsCorrectTagEnding(string tag, string line )
        {
            if (pointer >= line.Length)
                return false;
            if (pointer != 0 && line[pointer - 1] == '\\')
                return false;

            var previousChar = line[pointer - 1];
            return
                IsSameTag(line,tag) &&
                char.IsLetter(previousChar);
        }

        //public string Translate(string text)
        //{
        //    pointer = 0;
        //    var result = new StringBuilder();
        //    for (; pointer < text.Length; pointer++)
        //    {
        //        var currentTag = GetTag(text, pointer);
        //        if (currentTag != default(MarkdownTag))
        //        {
        //            if (tagsNesting.Any())
        //            {
        //                var previousTag = tagsNesting.Peek();
        //                if (currentTag != previousTag && previousTag.CanContain(currentTag))
        //                    result.Append(ParseTagOpening(currentTag));
        //                else
        //                    result.Append(ParseTagEnding(tagsNesting.Pop()));
        //            }
        //            else if (HasCorrectEnding(currentTag.Tag, text, pointer + currentTag.Length))
        //                result.Append(ParseTagOpening(currentTag));
        //        }
        //        else
        //            result.Append(text[pointer]);
        //    }

        //    return result.ToString();
        //}

        //private string ParseTagOpening(MarkdownTag tag)
        //{
        //    pointer += tag.Length - 1;
        //    tagsNesting.Push(tag);
        //    return tag.OpenTagTranslation;
        //}

        //private string ParseTagEnding(MarkdownTag tag)
        //{
        //    pointer += tag.Length - 1;
        //    return tag.CloseTagTranslation;
        //}

        //private MarkdownTag GetTag(string line, int index)
        //{
        //    var possibleTags = tagsCollection
        //        .Where(tag => tag.StartsWith(line[index]));

        //    if (tagsNesting.Any(tag => tag.Tag == line.Substring(index, tag.Length)))
        //        return tagsNesting
        //            .First(tag => HasCorrectEnding(tag.Tag, line, index));

        //    return possibleTags.FirstOrDefault(t => IsCorrectTagOpening(t.Tag, line, index));
        //}

        //private bool IsCorrectTagOpening(string tag, string line, int startIndex)
        //{
        //    if (startIndex + tag.Length >= line.Length)
        //        return false;
        //    if (startIndex != 0 && line[startIndex - 1] == '\\')
        //        return false;

        //    var nextChar = line[startIndex + tag.Length];
        //    return
        //         line.Substring(startIndex, tag.Length) == tag &&
        //        char.IsLetter(nextChar);
        //}

        //private bool HasCorrectEnding(string tag, string line, int startIndex)
        //{
        //    while (startIndex < line.Length)
        //    {
        //        startIndex = line.IndexOf(tag, startIndex, StringComparison.CurrentCulture);
        //        if (startIndex == -1)
        //            return false;
        //        var previousChar = line[startIndex - 1];
        //        if (char.IsLetter(previousChar) &&
        //            line.Substring(startIndex, tag.Length) == tag)
        //            return true;
        //        startIndex++;
        //    }

        //    return false;
        //}
    }
}