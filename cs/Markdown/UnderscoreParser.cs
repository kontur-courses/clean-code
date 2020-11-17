using System.Collections.Generic;

namespace Markdown
{
    public abstract class UnderscoreParser : Parser
    {
        public const char UnderscoreSymbol = '_';
        public const string DoubleUnderscore = "__";
        private List<TagInfo> tagsInsideWord;
        private bool wordContainsDigits;
        protected int UnderscoreCounter;
        protected bool PreviousIsSpace;
        private bool hasWhiteSpace;

        protected UnderscoreParser()
        {
            tagsInsideWord = new List<TagInfo>();
            PreviousIsSpace = true;
        }

        protected void ParseOpeningUnderscore(int index)
        {
            if (TextEnded)
            {
                if (PreviousIndex < Markdown.Length)
                    TagInfo.AddText(Markdown.Substring(PreviousIndex));
                PreviousIndex = Markdown.Length;
                State = States.Pop();
            }
            else if (Markdown[index] == UnderscoreSymbol && !ShouldEscaped(Markdown[index]))
                UnderscoreCounter++;
            else if (char.IsWhiteSpace(Markdown[index]))
            {
                UnderscoreCounter = 0;
                BackslashCounter = 0;
                State = States.Pop();
            }
            else
            {
                OpenUnderscoreTag(index);
            }
        }

        private void ParseUnderscoreContent(int index)
        {
            if (TextEnded)
            {
                if (!PreviousIsSpace && UnderscoreCounter != 0)
                {
                    CloseUnderscoreTag(Markdown.Length - 1);
                    if (wordContainsDigits)
                    {
                        foreach (var tag in tagsInsideWord)
                            tag.ResetFormatting(true);
                    }
                }
                else
                {
                    TagInfo.AddText(Markdown.Substring(PreviousIndex));
                    TagInfo.ResetFormatting();
                    TagInfo = NestedTextInfos.Pop();
                    State = States.Pop();
                    PreviousIndex = Markdown.Length;
                }
            }
            else if (Markdown[index] == UnderscoreSymbol && !ShouldEscaped(Markdown[index]))
            {
                UnderscoreCounter++;
            }
            else
            {
                if (char.IsDigit(Markdown[index]))
                    wordContainsDigits = true;

                if (UnderscoreCounter != 0)
                {
                    if (!PreviousIsSpace)
                    {
                        CloseUnderscoreTag(index);
                    }
                    else if (PreviousIsSpace)
                    {
                        SetNewState(OpenUnderscoreTag);
                        State(index);
                    }
                }

                if (char.IsWhiteSpace(Markdown[index]))
                {
                    hasWhiteSpace = true;
                    UnderscoreCounter = 0;

                    if (TagInfo.InsideWord)
                    {
                        TagInfo.ResetFormatting();
                        State = States.Pop();
                    }

                    tagsInsideWord = new List<TagInfo>();
                }


                if (wordContainsDigits)
                {
                    foreach (var tag in tagsInsideWord)
                        tag.ResetFormatting(true);
                }

                PreviousIsSpace = char.IsWhiteSpace(Markdown[index]) ||
                                  Markdown[index] == '\\' && BackslashCounter % 2 == 0;
            }
        }

        private void OpenUnderscoreTag(int index)
        {
            var tag = UnderscoreCounter == 1 ? Tag.Italic : Tag.Bold;
            UnderscoreCounter = UnderscoreCounter == 1 ? 1 : 2;
            var length = index - UnderscoreCounter - PreviousIndex;
            if (length > 0)
                TagInfo.AddText(Markdown.Substring(PreviousIndex, length));

            PreviousIndex = index;
            UnderscoreCounter = 0;
            hasWhiteSpace = false;

            SetNewTextInfo(new TagInfo(tag));
            TagInfo.InsideWord = !PreviousIsSpace;
            State = ParseUnderscoreContent;
            State(index);
        }

        private void CloseUnderscoreTag(int index)
        {
            var tag = UnderscoreCounter == 1 ? Tag.Italic : Tag.Bold;
            if (!HasOpeningTag(tag, Markdown[index]))
            {
                if (TextEnded)
                    TagInfo.AddText(Markdown.Substring(PreviousIndex));
                if (char.IsWhiteSpace(Markdown[index]))
                    UnderscoreCounter = 0;
                else
                {
                    SetNewState(OpenUnderscoreTag);
                    State(index);
                }
            }
            else if (TagsIntersect(tag))
            {
                var (currentTag, conflictedTag) =
                    UnderscoreCounter == 1 ? (Tag.Italic, Tag.Bold) : (Tag.Bold, Tag.Italic);
                ResetFormattingForConflictingTags(currentTag, conflictedTag);
                if (TextEnded)
                {
                    TagInfo.AddText(Markdown.Substring(PreviousIndex));
                    PreviousIndex = Markdown.Length;
                }
            }
            else
            {
                if (TagInfo.Tag == Tag.Italic || TextEnded && tag == Tag.Italic)
                {
                    while (TagInfo.Tag != Tag.Italic)
                        TagInfo = NestedTextInfos.Pop();
                    foreach (var bold in TagInfo.FindAndGetBoldContent())
                        bold.ResetFormatting(true);
                }

                CloseFirstMatching(index);
            }
        }

        private bool HasOpeningTag(Tag tag, char currentSymbol)
        {
            var result = false;
            if (TagInfo.Tag == tag && IsValidTag(TagInfo, currentSymbol))
                return true;

            var temporaryStack = new Stack<TagInfo>();
            while (NestedTextInfos.Count != 0)
            {
                var tagInfo = NestedTextInfos.Pop();
                if (tagInfo.Tag == tag && IsValidTag(tagInfo, currentSymbol))
                {
                    NestedTextInfos.Push(tagInfo);
                    result = true;
                    break;
                }

                temporaryStack.Push(tagInfo);
            }

            while (temporaryStack.Count != 0)
                NestedTextInfos.Push(temporaryStack.Pop());

            return result;
        }

        private bool IsValidTag(TagInfo tagInfo, char currentSymbol)
        {
            return !tagInfo.InsideWord && (char.IsWhiteSpace(currentSymbol) || TextEnded) || !hasWhiteSpace;
        }

        private void CloseFirstMatching(int index)
        {
            var tag = UnderscoreCounter == 1 ? Tag.Italic : Tag.Bold;
            var tagInfo = TagInfo;
            var temporaryStack = new Stack<TagInfo>();
            do
            {
                if (tagInfo.Tag == tag)
                {
                    var length = TextEnded
                        ? index - PreviousIndex - UnderscoreCounter + 1
                        : index - PreviousIndex - UnderscoreCounter;
                    tagInfo.AddText(Markdown.Substring(PreviousIndex, length));
                    tagInfo.InsideWord = !PreviousIsSpace;
                    tagInfo.IsClosed = true;

                    State = States.Pop();
                    PreviousIndex = UnderscoreCounter < 3 ? index : index - UnderscoreCounter + 2;
                    UnderscoreCounter = 0;

                    if (TextEnded) PreviousIndex++;
                    if (!PreviousIsSpace || TextEnded) tagsInsideWord.Add(TagInfo);
                    break;
                }

                temporaryStack.Push(tagInfo);
                tagInfo = NestedTextInfos.Pop();
            } while (NestedTextInfos.Count != 0);

            if (temporaryStack.Count > 0)
            {
                while (temporaryStack.Count > 0)
                    NestedTextInfos.Push(temporaryStack.Pop());
            }

            TagInfo = NestedTextInfos.Pop();
        }

        private bool TagsIntersect(Tag tag)
        {
            return tag == Tag.Italic && TagInfo.Tag == Tag.Bold ||
                   tag == Tag.Bold && TagInfo.Tag == Tag.Italic;
        }

        private void ResetFormattingForConflictingTags(Tag currentTag, Tag conflictedTag)
        {
            TagInfo.ResetFormatting();
            State = States.Pop();

            var temporaryStack = new Stack<TagInfo>();
            TagInfo tagInfo;
            do
            {
                tagInfo = NestedTextInfos.Pop();
                if (tagInfo.Tag != conflictedTag && tagInfo.Tag != currentTag)
                    break;

                tagInfo.ResetFormatting();
                temporaryStack.Push(tagInfo);
                State = States.Pop();

                if (currentTag == tagInfo.Tag)
                    break;
            } while (currentTag != tagInfo.Tag);

            TagInfo = tagInfo;
        }
    }
}