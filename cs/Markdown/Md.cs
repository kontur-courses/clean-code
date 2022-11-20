using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.NetworkInformation;

namespace Markdown
{
    public class Md
    {
        private readonly Dictionary<TagType, TagInfo> tags;

        public Md()
        {
            tags = new Dictionary<TagType, TagInfo>
            {
                [TagType.OneUnderscore] = new TagInfo("_", "<em>", "</em>"),
                [TagType.TwoUnderscore] = new TagInfo("__", "<strong>", "</strong>"),
                [TagType.Header] = new TagInfo("# ", "<h1>", "</h1>"),
                [TagType.Paragraph] = new TagInfo("", "<p>", "</p>")
            };
        }

        public string Render(string str)
        {
            var partsText = DivideIntoParts(str);
            var htmlStr = new StringBuilder();
            htmlStr.Append(tags[TagType.Paragraph].SymbolOpen);
            var headerClosed = "";
            for (var i=0;i<partsText.Count;i++)
            {
                if (partsText[i].Tag is null || partsText[i].Tag.Status == TagStatus.NoOpen)
                    htmlStr.Append(partsText[i].Text);
                
                else if (partsText[i].Tag.TagType == TagType.OneUnderscore ||
                         partsText[i].Tag.TagType == TagType.TwoUnderscore)
                {
                    if (partsText[i].Tag.Status == TagStatus.Open)
                    {
                        var tag = InteractionTag(partsText[i].Tag, partsText, i + 1);
                        htmlStr.Append(tag.Status == TagStatus.Open ? tags[tag.TagType].SymbolOpen : partsText[i].Text);
                    }
                    else
                        htmlStr.Append(tags[partsText[i].Tag.TagType].SymbolClose);
                }

                else if (partsText[i].Tag.TagType == TagType.Header)
                {
                    htmlStr.Append(tags[TagType.Header].SymbolOpen);
                    headerClosed += tags[TagType.Header].SymbolClose;
                }
            }
            htmlStr.Append(headerClosed + tags[TagType.Paragraph].SymbolClose);
            return htmlStr.ToString();
        }

        private Tag InteractionTag(Tag tag, List<PartText> partsText, int numberText)
        {
            var countTag = 0;
            var closedTagNumber=0;
            for (var i = numberText; i < partsText.Count; i++)
            {
                if(!(partsText[i].Tag is null) && partsText[i].Tag.TagType==tag.TagType && partsText[i].Tag.Status != TagStatus.NoOpen)
                {
                    closedTagNumber = i;
                    break;
                }
                else if (!(partsText[i].Tag is null) && partsText[i].Tag.Status != TagStatus.NoOpen)
                    countTag++;
            }

            if (countTag % 2 == 1 || tag.TagType == TagType.OneUnderscore)
            {
                for (var i = numberText; i < closedTagNumber; i++)
                {
                    if (!(partsText[i].Tag is null))
                    {
                        if (partsText[i].Tag.Status == TagStatus.Open)
                            partsText[i].Tag.ClosedTag.Status = TagStatus.NoOpen;
                        partsText[i].Tag.Status = TagStatus.NoOpen;
                    }
                }
            }

            if (countTag % 2 == 1)
            {
                tag.Status = TagStatus.NoOpen;
                tag.ClosedTag.Status = TagStatus.NoOpen;
            }
            return tag;
        }

        private List<PartText> DivideIntoParts(string str)
        {
            var partsText = new List<PartText>();
            var lastOpen = new Tag(TagType.OneUnderscore, TagStatus.NoOpen);
            var lastOpenTwo = new Tag(TagType.TwoUnderscore, TagStatus.NoOpen);
            var htmlStr = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if (i == str.Length - 1 && !(lastOpen.Status == TagStatus.Open && IsEnd(str[i - 1], str[i])))
                    htmlStr.Append(str[i]);

                else if (str[i] == '#' && char.IsWhiteSpace(str[i + 1]))
                {
                    partsText.Add(new PartText("# ", new Tag(TagType.Header)));
                    i++;
                }

                else if (lastOpenTwo.Status == TagStatus.Open && str[i] == '_' && i < str.Length-1  && IsEnd(str[i - 1], str[i + 1]))
                {
                    i++;
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    if (i < str.Length - 1 && char.IsLetter(str[i + 1]))
                        lastOpenTwo.HasLetterAfter = true;
                    var tag = GetClosedTag(lastOpenTwo);
                    if (!(tag is null))
                        lastOpenTwo = tag;
                    partsText.Add(new PartText("__", tag));
                }

                else if ((lastOpenTwo.Status == TagStatus.NoOpen || lastOpenTwo.Status == TagStatus.Close) && str[i] == '_' 
                         && i < str.Length-2  && IsBegin(str[i + 1], str[i + 2]))
                {
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    var tag = new Tag(TagType.TwoUnderscore, TagStatus.Open);
                    lastOpenTwo = tag;
                    if (i != 0 && char.IsLetter(str[i - 1]))
                        lastOpenTwo.HasLetterBefore = true;
                    partsText.Add(new PartText("__", lastOpenTwo));
                    i++;
                }

                else if (lastOpen.Status == TagStatus.Open && IsEnd(str[i - 1], str[i]))
                {
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    if(i < str.Length - 1 && char.IsLetter(str[i + 1]))
                        lastOpen.HasLetterAfter = true;
                    var tag = GetClosedTag(lastOpen);
                    if (!(tag is null))
                        lastOpen = tag;
                    partsText.Add(new PartText("_", tag));
                }

                else if ((lastOpen.Status == TagStatus.NoOpen || lastOpen.Status == TagStatus.Close) && IsBegin(str[i], str[i+1]))
                {
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    var tag = new Tag(TagType.OneUnderscore, TagStatus.Open);
                    lastOpen = tag;
                    if (i != 0 && char.IsLetter(str[i - 1]))
                        lastOpen.HasLetterBefore = true;
                    partsText.Add(new PartText("_", lastOpen));
                }

                else if (IsEscaped(str[i], str[i + 1]))
                {
                    htmlStr.Append(str[i + 1]);
                    i++;
                }

                else 
                {
                    htmlStr.Append(str[i]);
                    if (lastOpen.Status == TagStatus.Open && char.IsWhiteSpace(str[i]))
                        lastOpen.HasSpaceBetween = true;
                    if (lastOpenTwo.Status == TagStatus.Open && char.IsWhiteSpace(str[i]))
                        lastOpenTwo.HasSpaceBetween = true;
                }
            }

            if (htmlStr.Length > 0)
                partsText.Add(new PartText(htmlStr.ToString()));
            if (lastOpen.Status == TagStatus.Open)
                lastOpen.Status = TagStatus.NoOpen;
            if (lastOpenTwo.Status == TagStatus.Open)
                lastOpenTwo.Status = TagStatus.NoOpen;
            return partsText;
        }

        private Tag GetClosedTag(Tag lastOpen) 
        {
            Tag tag = null;
            if (!lastOpen.HasLetterAfter && !lastOpen.HasLetterBefore || !lastOpen.HasSpaceBetween)
            {
                tag = new Tag(lastOpen.TagType, TagStatus.Close, lastOpen);
                lastOpen.ClosedTag = tag;
            }
            else
                lastOpen.Status = TagStatus.NoOpen;
            return tag;
        }

        private bool IsBegin(char ch, char nextCh)
        {
            if (ch != '_') return false;
            return (!char.IsDigit(nextCh) && !char.IsWhiteSpace(nextCh) && nextCh != '_');
        }

        private bool IsEnd(char prevCh, char ch)
        {
            if (ch != '_') return false;
            return (!char.IsWhiteSpace(prevCh) && prevCh != '_');
        }

        private bool IsEscaped(char ch, char nextCh)
        {
            if (ch != '\\') return false;
            var chCanEscaped = new[] { '\\', '_', '#' };
            return chCanEscaped.Any(x => x == nextCh);
        }
    }
}
