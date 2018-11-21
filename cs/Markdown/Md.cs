using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly Tag[] tags;
        private readonly char[] forbiddenSymbols;

        public Md(IEnumerable<Tag> tags, char[] forbiddenSymbols)
        {
            this.tags = tags.ToArray();
            this.forbiddenSymbols = forbiddenSymbols;
        }

        public string Render(string paragraph)
        {
            var text = new StringBuilder(paragraph);
            var markersQueue = FillTagMarkers(text);
            var markerPairs = ConvertToPairs(markersQueue);
            markerPairs = RemoveUnpairedTags(markerPairs);
            return ReplaceTags(text, markerPairs);
        }

        private List<MarkerPair> RemoveUnpairedTags(List<MarkerPair> markerPairs)
            => markerPairs.Where(p => p.Closer != null && p.Opener != null).ToList();

        private List<MarkerPair> ConvertToPairs(Queue<Marker> tagMarkersQueue)
        {
            var result = new List<MarkerPair>();
            while (tagMarkersQueue.Count > 0)
            {
                var marker = tagMarkersQueue.Dequeue();
                var openerMarker = result.FirstOrDefault(m => m.Closer == null && m.Opener.Tag.Equals(marker.Tag));
                if (openerMarker == null)
                {
                    var tagPair = new MarkerPair(marker, null);
                    result.Add(tagPair);
                }
                else
                {
                    marker.TagType = TagType.Closer;
                    openerMarker.Closer = marker;
                }
            }

            return result;
        }

        private Queue<Marker> FillTagMarkers(StringBuilder text)
        {
            var markersQueue = new Queue<Marker>();
            for (var i = 0; i < text.Length; i++)
            {
                var currentSymbol = text[i].ToString();
                if (IsEscape(text.ToString(), i))
                {
                    text.Remove(i, 1);
                    continue;
                }

                if (IsSpecialSymbol(currentSymbol) && !HaveForbiddenSymbolsAround(text, i))
                {
                    var tag = tags.FirstOrDefault(t => t.IsMd(text.ToString(), i));
                    if (tag == null)
                        continue;
                    AddMarker(tag, text, markersQueue, i);
                    i += tag.Md.Length;
                }
            }

            return markersQueue;
        }

        private bool HaveForbiddenSymbolsAround(StringBuilder text, int position)
        {
            if (position == 0)
                return HasForbiddenAfter(text, position);
            if (position == text.Length - 1)
                return HasForbiddenBefore(text, position);
            return HasForbiddenBefore(text, position) || HasForbiddenAfter(text, position);
        }

        private bool HasForbiddenBefore(StringBuilder text, int position)
            => forbiddenSymbols.Contains(text[position - 1]);

        private bool HasForbiddenAfter(StringBuilder text, int position)
            => forbiddenSymbols.Contains(text[position + 1]);

        private void AddMarker(Tag tag, StringBuilder text, Queue<Marker> markersQueue, int position)
        {
            if (!HasWhitespaceAfter(text, position) && !IsIgnoringTag(markersQueue, tag))
            {
                var marker = new Marker(tag, TagType.Opener, position);
                markersQueue.Enqueue(marker);
            }
            else if (!HasWhitespaceBefore(text, position) && !IsIgnoringTag(markersQueue, tag))
            {
                var marker = new Marker(tag, TagType.Closer, position);
                markersQueue.Enqueue(marker);
            }
        }

        private bool IsIgnoringTag(Queue<Marker> markersQueue, Tag tag)
        {
            if (markersQueue.Count == 0)
                return false;
            var lastMarker = markersQueue.Peek();
            return lastMarker.TagType == TagType.Opener && lastMarker.Tag.IsIgnoringMd(tag.Md);
        }

        private bool HasWhitespaceAfter(StringBuilder text, int position)
            => position == text.Length - 1 || char.IsWhiteSpace(text[position + 1]);

        private bool HasWhitespaceBefore(StringBuilder text, int position)
            => position == 0 || char.IsWhiteSpace(text[position - 1]);

        private bool IsSpecialSymbol(string symbol)
            => tags.Any(t => t.ContainsMdSymbols(symbol));

        private List<Marker> PresentAsListOfMarkers(IEnumerable<MarkerPair> markerPairs)
        {
            var markers = new List<Marker>();
            foreach (var pair in markerPairs)
            {
                markers.Add(pair.Opener);
                markers.Add(pair.Closer);
            }

            return markers;
        }

        private string ReplaceTags(StringBuilder text, IEnumerable<MarkerPair> markerPairs)
        {
            var result = new StringBuilder(text.ToString());
            var markers = PresentAsListOfMarkers(markerPairs);
            var sortedMarkers = markers.OrderByDescending(m => m.Position);
            foreach (var marker in sortedMarkers)
            {
                var lengthToRemove = marker.Tag.Md.Length;
                result.Remove(marker.Position, lengthToRemove);
                var tagToInsert = GetTagToInsert(marker);
                result.Insert(marker.Position, tagToInsert);
            }

            return result.ToString();
        }

        private string GetTagToInsert(Marker marker)
            => marker.TagType == TagType.Opener ? marker.Tag.Html : marker.Tag.CloserHtml;

        private bool IsEscape(string line, int position)
            => line[position] == '\\';
    }
}
