using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class TagInserter
    {

        public string Insert(string text, List<Tag> tags)
        {
            tags.Sort((x, y) => x.Position.CompareTo(y.Position));
            StringBuilder outText = new StringBuilder();
            var currentTextIndex = 0;
            var currentTagsIndex = 0;
            var linkTagIndex = -1;
            while (currentTagsIndex < tags.Count)
            {
                var tag = tags[currentTagsIndex];
                outText.Append(text.Substring(currentTextIndex, tag.Position - currentTextIndex));
                currentTextIndex = tag.Position;
                switch (tag.Type)
                {
                    case TagType.Em:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.Em].Length;
                        //currentTextIndex++;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.EmClose:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.EmClose].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.Strong:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.Strong].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.StrongClose:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.StrongClose].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.S:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.S].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.SClose:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.SClose].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.Backslash:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.Backslash].Length;
                        break;
                    case TagType.A:
                        if (!CheckLinkAvailability(currentTagsIndex, tags))
                            break;
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.A].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        linkTagIndex = outText.Length-1;
                        break;
                    case TagType.AClose:
                        if (!CheckLinkAvailabilityForClosedA(currentTagsIndex, tags))
                            break;
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.AClose].Length;
                        outText.Append(Tag.TagTextRepresentation[tag.Type]);
                        break;
                    case TagType.LinkBracket:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.LinkBracket].Length;
                        while (tags[currentTagsIndex].Type != TagType.LinkBracketClose)
                            currentTagsIndex++;
                        outText.Insert(linkTagIndex, " href=\"" + text.Substring(currentTextIndex, tags[currentTagsIndex].Position - currentTextIndex) + "\"");
                        currentTextIndex = tags[currentTagsIndex].Position;
                        currentTextIndex++;
                        break;
                    case TagType.LinkBracketClose:
                        currentTextIndex += Tag.TagMarkerTextRepresentation[TagType.LinkBracketClose].Length;
                        break;
                }

                currentTagsIndex++;
            }
            outText.Append(text.Substring(currentTextIndex));
            return outText.ToString();
        }

        private bool CheckLinkAvailability(int index, List<Tag> tags)
        {
            var closeSquareBracketFound = false;
            for (int i = index; i < tags.Count; i++)
            {
                if (!closeSquareBracketFound)
                    if (tags[i].Type == TagType.AClose)
                        closeSquareBracketFound = true;
                if (closeSquareBracketFound)
                    return CheckLinkAvailabilityForClosedA(i, tags);
            }
            return false;
        }

        private bool CheckLinkAvailabilityForClosedA(int index, List<Tag> tags)
        {
            var OpenBracketFound = false;
            var closeBracketFound = false;
            for (int i = index; i < tags.Count; i++)
            {
                if (!OpenBracketFound)
                    if (tags[i].Type == TagType.LinkBracket)
                        OpenBracketFound = true;
                if (!closeBracketFound)
                    if (tags[i].Type == TagType.LinkBracketClose)
                        closeBracketFound = true;
                if (OpenBracketFound && closeBracketFound)
                    return true;
            }
            return false;
        }
    }
}
