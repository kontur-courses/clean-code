using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private TagKeeper[] tags;
        private Stack<TagMarker> mdTagsBuffer;
        private StringBuilder strBuilder;
        private string incorrectSymbols = " 0123456789";

        public Md(IEnumerable<TagKeeper> tags)
        {
            this.tags = tags.ToArray();
            mdTagsBuffer = new Stack<TagMarker>();
        }

        public string Render(string paragraph)
        {
            strBuilder = new StringBuilder(paragraph);
            var specialSymbols = new StringBuilder();
            int? position = null;
            for (var i = 0; i < strBuilder.Length; i++)
            {
                var currentSymbol = strBuilder[i].ToString();
                if (IsEscape(strBuilder, i))
                {
                    strBuilder.Remove(i, 1);
                    continue;
                }
                if (IsSpecialSymbol(currentSymbol))
                {
                    if (position == null)
                        position = i;
                    specialSymbols.Append(currentSymbol);
                }
                else
                {
                    if (position == null)
                        continue;
                    AddMdTagMarker(specialSymbols.ToString(), (int)position);
                    specialSymbols.Clear();
                    ReplaceTags(strBuilder);
                    position = null;
                }
            }

            if (position != null)
            {
                AddMdTagMarker(specialSymbols.ToString(), (int) position);
                specialSymbols.Clear();
                ReplaceTags(strBuilder);
            }

            return strBuilder.ToString();
        }

        private void AddMdTagMarker(string mdTag, int position)
        {
            var tagKeeper = tags.FirstOrDefault(t => t.Is(mdTag));
            if (tagKeeper == null)
                return;
            var marker = new TagMarker(tagKeeper, position);
            mdTagsBuffer.Push(marker);
        }

        private bool IsSpecialSymbol(string symbol)
            => tags.Any(t => t.ContainsMd(symbol));

        private void ReplaceTags(StringBuilder line)
        {
            var markersToChange = FindMarkersToChange();
            if (markersToChange == null)
                return;
            ReplaceTag(line, markersToChange.Item2, true);
            ReplaceTag(line, markersToChange.Item1);
        }

        private void ReplaceTag(
            StringBuilder line, 
            TagMarker tagMarker, 
            bool isCloser = false)
        {
            var lengthToRemove = tagMarker.TagKeeper.Md.Value.Length;
            var position = tagMarker.Position;
            var htmlTag = tagMarker.TagKeeper.Html.Value;
            if (isCloser)
                htmlTag = htmlTag.Insert(1, "/");
            line.Remove(position, lengthToRemove);
            line.Insert(position, htmlTag);
        }

        private Tuple<TagMarker, TagMarker> FindMarkersToChange()
        {
            var tagToChange = mdTagsBuffer.Pop();
            if (!IsCloserTag(tagToChange))
            {
                mdTagsBuffer.Push(tagToChange);
                return null;
            }

            if (!IsCorrectCloser(strBuilder, tagToChange.Position))
                return null;
            while (mdTagsBuffer.Count > 0)
            {
                var currentTag = mdTagsBuffer.Pop();
                if (!currentTag.TagKeeper.Is(tagToChange.TagKeeper))
                    continue;
                if (!IsCorrectOpener(strBuilder, currentTag.Position, currentTag.TagKeeper.Md.Value.Length))
                    return null;
                return new Tuple<TagMarker, TagMarker>(currentTag, tagToChange);
            }

            return null;
        }

        private bool IsCloserTag(TagMarker marker)
            => mdTagsBuffer.Any(m => m.TagKeeper.Equals(marker.TagKeeper));

        private bool IsCorrectOpener(StringBuilder line, int position, int length)
            => !incorrectSymbols.Contains(line[position + length]);

        private bool IsCorrectCloser(StringBuilder line, int position)
            => !incorrectSymbols.Contains(line[position - 1]);

        private bool IsEscape(StringBuilder line, int position)
            => line[position] == '\\';
    }
}
