using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework.Api;

namespace Markdown
{
    public class Md
    {
        private readonly Dictionary<string,ITag> tags;

        public Md()
        {
            tags = new Dictionary<string, ITag>
            {
                ["__"] = new TagTwoUnderscore(),
                ["_"] = new TagOneUnderscore(),
                ["# "] = new TagHeader()
            };
        }

        public string Render(string str)
        {
            var partsText = DivideIntoParts(str);
            partsText  = ReplaceAllInteractionUnderscoreTag(partsText);
            return Translator.Translate(partsText);
        }

        private List<PartText> DivideIntoParts(string str)
        {
            var partsText = new List<PartText>();
            var text = new StringBuilder();
            str = " " + str + " ";
            
            for (var i = 1; i < str.Length - 1; i++)
            {
                if (IsEscaped(str[i], str[i + 1]))
                {
                    text.Append(str[i + 1]);
                    i++;
                    continue;
                }

                var partText = TryGetPartTextWithTag(str, i);
                if (partText is null)
                {
                    text.Append(str[i]);
                    if (char.IsWhiteSpace(str[i]))
                        TagSetIsSpaceBetween();
                }
                else
                {
                    if (text.Length > 0)
                    {
                        partsText.Add(new PartText(text.ToString()));
                        text.Clear();
                    }
                    partsText.Add(partText);
                    i += partText.Text.Length - 1;
                }
            }

            if (text.Length > 0)
                partsText.Add(new PartText(text.ToString()));

            var part = MakePartWithCloseTag();
            if (!(part is null))
                partsText.Add(part);

            return partsText;
        }

        private bool IsEscaped(char ch, char nextCh)
        {
            if (ch != '\\') return false;
            var chCanEscaped = new[] { '\\', '_', '#' };
            return chCanEscaped.Any(x => x == nextCh);
        }

        private PartText TryGetPartTextWithTag(string str, int position)
        {
            var twoChar = string.Concat(str[position], str[position + 1]);
            foreach (var tagType in tags.OrderByDescending(t => t.Key.Length))
            {
                if (twoChar.StartsWith(tagType.Key))
                {
                    var tag = tagType.Value;
                    position += tagType.Key.Length - 1;
                    if (tag.Tag.Status == TagStatus.Open && tag.IsEnd(str[position - tagType.Key.Length]))
                        return new PartText(tagType.Key, tag.GetClosedTag(str[position + 1]));
                    else if (tag.Tag.Status != TagStatus.Open && tag.IsBegin(str[position + 1]))
                        return new PartText(tagType.Key, tag.GetOpenTag(str[position - tagType.Key.Length]));
                    else
                        return new PartText(tagType.Key);
                }
            }
            return null;
        }

        private void TagSetIsSpaceBetween()
        {
            foreach (var tag in tags)
            {
                if (tag.Value.Tag.Status == TagStatus.Open)
                    tag.Value.SetIsSpaceBetween();
            }
        }

        private PartText MakePartWithCloseTag()
        {
            foreach (var tag in tags)
            {
                if (tag.Value.Tag.Status == TagStatus.Open)
                {
                    if (tag.Value.ShouldClose)
                        tag.Value.Tag.Status = TagStatus.NoOpen;
                    else
                        return new PartText(tag.Key, tag.Value.GetClosedTag());
                }
            }
            return null;
        }

        private List<PartText> ReplaceAllInteractionUnderscoreTag(List<PartText> partsText)
        {
            var queueTag = new Queue<Tag>(partsText
                .Select(p => p.Tag)
                .Where(t => !(t is null) && t.Status != TagStatus.NoOpen && (t.TagType == TagType.OneUnderscore || t.TagType == TagType.TwoUnderscore)));
            if (queueTag.Count != 0)
                ReplaceInteractionUnderscoreTag(queueTag);
            return partsText;
        }

        private Queue<Tag> ReplaceInteractionUnderscoreTag(Queue<Tag> queueTag)
        {
            if (queueTag.Count == 0)
                return queueTag;

            var tag = queueTag.Dequeue();
            if (tag.Status != TagStatus.Open)
                return ReplaceInteractionUnderscoreTag(queueTag);

            var innerTags = new List<Tag>();

            foreach (var innerTag in queueTag)
            {
                if (innerTag.TagType == tag.TagType)
                    break;
                innerTags.Add(innerTag);
            }

            var countTag = innerTags.Count;
            if (countTag % 2 == 1 || tag.TagType == TagType.OneUnderscore)
            {
                foreach (var innerTag in innerTags)
                {
                    if (innerTag.Status == TagStatus.Open)
                        innerTag.ClosedTag.Status = TagStatus.NoOpen;
                    innerTag.Status = TagStatus.NoOpen;
                }
            }

            if (countTag % 2 == 1)
            {
                tag.Status = TagStatus.NoOpen;
                tag.ClosedTag.Status = TagStatus.NoOpen;
            }
            return ReplaceInteractionUnderscoreTag(queueTag);
        }
    }
}
