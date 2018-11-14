using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private TagKeeper[] tags;
        private Stack<TagMarker> mdTagsBuffer;

        public Md(IEnumerable<TagKeeper> tags)
        {
            this.tags = tags.ToArray();
            mdTagsBuffer = new Stack<TagMarker>();
        }

        public string Render(string paragraph)
        {
            var strBuilder = new StringBuilder(paragraph);
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
            var tagKeeper = tags.First(t => t.Is(mdTag));
            if (tagKeeper == null)
                return;
            var marker = new TagMarker(tagKeeper, position);
            mdTagsBuffer.Push(marker);
        }

        private bool IsSpecialSymbol(string symbol)
            => tags.Any(t => t.Contains(symbol));

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
            if (!mdTagsBuffer.Any(m => m.TagKeeper.Equals(tagToChange.TagKeeper)))
            {
                mdTagsBuffer.Push(tagToChange);
                return null;
            }
            while (mdTagsBuffer.Count > 0)
            {
                var currentTag = mdTagsBuffer.Pop();
                if (!currentTag.TagKeeper.Is(tagToChange.TagKeeper))
                    continue;
                return new Tuple<TagMarker, TagMarker>(currentTag, tagToChange);
            }

            return null;
        }

        private bool IsEscape(StringBuilder line, int position)
            => line[position] == '\\';
    }
}
