using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string str)
        {
            var partsText = DivideIntoParts(str);
            var htmlStr = new StringBuilder();
            htmlStr.Append("<p>");
            var headerClosed = "";
            foreach (var item in partsText)
            {
                if (item.Tag is null || item.Tag.Status==TagStatus.NoOpen)
                    htmlStr.Append(item.Text);
                else if (item.Tag.TagType == TagType.OneUnderscore && item.Tag.Status == TagStatus.Open)
                {
                    InteractionTagOne(item.Tag);
                    htmlStr.Append(item.Tag.Status == TagStatus.Open ? "<em>" : item.Text);
                }
                else if (item.Tag.TagType == TagType.OneUnderscore && item.Tag.Status == TagStatus.Close)
                    htmlStr.Append("</em>");
                else if (item.Tag.TagType == TagType.TwoUnderscore && item.Tag.Status == TagStatus.Open)
                {
                    InteractionTagTwo(item.Tag);
                    htmlStr.Append(item.Tag.Status == TagStatus.Open ? "<strong>" : item.Text);
                }
                else if (item.Tag.TagType == TagType.TwoUnderscore && item.Tag.Status == TagStatus.Close)
                    htmlStr.Append("</strong>");
                else if (item.Tag.TagType == TagType.Header)
                {
                    htmlStr.Append("<h1>");
                    headerClosed += "</h1>";
                }
            }
            htmlStr.Append(headerClosed+"</p>");
            return htmlStr.ToString();
        }

        void InteractionTagOne(Tag tag)
        {
            var countTag = 0;
            foreach (var innerTag in tag.InnerTag)
            {
                if (innerTag.Status != TagStatus.NoOpen)
                    countTag++;
            }
            foreach (var innerTag in tag.InnerTag)
            {
                if (innerTag.Status == TagStatus.Open)
                    innerTag.ClosedTag.Status = TagStatus.NoOpen;
                innerTag.Status = TagStatus.NoOpen;
            }
            if (countTag%2==1)
            {
                tag.Status = TagStatus.NoOpen;
                tag.ClosedTag.Status = TagStatus.NoOpen;
            }
        }

        void InteractionTagTwo(Tag tag)
        {
            var countTag = 0;
            foreach (var innerTag in tag.InnerTag)
            {
                if (innerTag.Status != TagStatus.NoOpen)
                    countTag++;
            }
            if (countTag % 2 == 1)
            {
                foreach (var innerTag in tag.InnerTag)
                {
                    if (innerTag.Status == TagStatus.Open)
                        innerTag.ClosedTag.Status = TagStatus.NoOpen;
                    innerTag.Status = TagStatus.NoOpen;
                }

                tag.Status = TagStatus.NoOpen;
                tag.ClosedTag.Status = TagStatus.NoOpen;
            }
        }


        public List<PartText> DivideIntoParts(string str)
        {
            var partsText = new List<PartText>();
            Tag lastOpen = new Tag(TagType.OneUnderscore,TagStatus.NoOpen);
            Tag lastOpenTwo = new Tag(TagType.TwoUnderscore,TagStatus.NoOpen);
            var htmlStr = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if (i == str.Length - 1 && !(lastOpen.Status == TagStatus.Open && IsEnd(str[i - 1], str[i])))
                {
                    htmlStr.Append(str[i]);
                }

                else if (str[i] == '#' && char.IsWhiteSpace(str[i + 1]))
                {
                    partsText.Add(new PartText("#", new Tag(TagType.Header)));
                    i++;
                }
                else if (lastOpenTwo.Status == TagStatus.Open && str[i] == '_' && i < str.Length-1  && IsEnd(str[i - 1], str[i + 1]))
                {
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    if (i < str.Length - 2 && char.IsLetter(str[i + 2]))
                        lastOpenTwo.HasLetterAfter = true;
                    if (!lastOpenTwo.HasLetterAfter && !lastOpenTwo.HasLetterBefore || !lastOpenTwo.HasSpaceBetween)
                    {
                        var tag = new Tag(TagType.TwoUnderscore,TagStatus.Close, lastOpenTwo);
                        partsText.Add(new PartText("__", tag));
                        lastOpenTwo.ClosedTag = tag;
                        lastOpenTwo = tag;
                        i++;
                        if (lastOpen.Status == TagStatus.Open)
                            lastOpen.InnerTag.Add(tag);
                    }
                    else
                    {
                        partsText.Add(new PartText("__"));
                        lastOpenTwo.Status = TagStatus.NoOpen;
                    }
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
                    if (lastOpen.Status == TagStatus.Open)
                        lastOpen.InnerTag.Add(tag);
                }

                else if (lastOpen.Status == TagStatus.Open && IsEnd(str[i - 1], str[i]))
                {
                    partsText.Add(new PartText(htmlStr.ToString()));
                    htmlStr.Clear();
                    if(i != str.Length - 1 && char.IsLetter(str[i + 1]))
                        lastOpen.HasLetterAfter = true;
                    if (!lastOpen.HasLetterAfter && !lastOpen.HasLetterBefore || !lastOpen.HasSpaceBetween)
                    {
                        var tag = new Tag(TagType.OneUnderscore,TagStatus.Close, lastOpen);
                        partsText.Add(new PartText("_", tag));
                        lastOpen.ClosedTag = tag;
                        lastOpen = tag;
                        if (lastOpenTwo.Status == TagStatus.Open)
                            lastOpenTwo.InnerTag.Add(tag);
                    }
                    else
                    {
                        partsText.Add(new PartText("_"));
                        lastOpen.Status = TagStatus.NoOpen;
                    }
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
                    if (lastOpenTwo.Status == TagStatus.Open)
                        lastOpenTwo.InnerTag.Add(tag); 
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
