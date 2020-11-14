using System.Collections.Generic;

namespace Markdown
{
    public abstract class UnderscoreParser : Parser
    {
        protected int UnderscoreCounter;
        private bool previousIsSpace;
        private int underscoresCountInWord;

        protected void ParseOpeningUnderscore(int index)
        {
            if (TextEnded)
            {
                if (PreviousIndex < Markdown.Length)
                    TextInfo.AddText(Markdown.Substring(PreviousIndex));
                PreviousIndex = Markdown.Length;
                State = States.Pop();
            }
            else if (Markdown[index] == '_')
                UnderscoreCounter++;
            else if (char.IsWhiteSpace(Markdown[index]))
            {
                UnderscoreCounter = 0;
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
                if (!previousIsSpace && (UnderscoreCounter == 2 || UnderscoreCounter == 1))
                    CloseUnderscoreTag(Markdown.Length - 1);
                else
                {
                    TextInfo.AddText(Markdown.Substring(PreviousIndex));
                    TextInfo.ToNoFormatting();
                    State = States.Pop();
                }
                PreviousIndex = Markdown.Length;
            }
            else if (Markdown[index] == '_')
            {
                UnderscoreCounter++;
            }
            else
            {
                if (UnderscoreCounter != 0)
                {
                    if (char.IsWhiteSpace(Markdown[index]))
                    {
                        CloseUnderscoreTag(index);
                    }
                    else
                    {
                        SetNewState(OpenUnderscoreTag);
                        State(index);
                    }
                }
                
                if (!char.IsWhiteSpace(Markdown[index]))
                {
                    previousIsSpace = false;
                    WordStartIndex = index;
                    SetNewState(ParseInsideWord);
                }

                previousIsSpace = char.IsWhiteSpace(Markdown[index]);
            }
        }

        private void OpenUnderscoreTag(int index)
        {
            var tag = UnderscoreCounter == 1 ? Tag.Italic : Tag.Bold;
            UnderscoreCounter = UnderscoreCounter == 1 ? 1 : 2;

            var length = index - UnderscoreCounter - PreviousIndex;
            if (length > 0)
                TextInfo.AddText(Markdown.Substring(PreviousIndex, length));

            PreviousIndex = index;
            UnderscoreCounter = 0;

            State = ParseUnderscoreContent;
            SetNewTextInfo(new TextInfo(tag));
            State(index);
        }

        private void CloseUnderscoreTag(int index)
        {
            if (!HasOpeningTag())
            {
                if (TextEnded)
                    TextInfo.AddText(Markdown.Substring(PreviousIndex));
                UnderscoreCounter = 0;
                return;
            }
            
            if (TagsAreIntersects())
            {
                var (currentTag, conflictedTag) =
                    UnderscoreCounter == 1 ? (Tag.Italic, Tag.Bold) : (Tag.Bold, Tag.Italic);
                ChangeAllConflictedTagsToNoFormatting(currentTag, conflictedTag);
                if (TextEnded)
                    TextInfo.AddText(Markdown.Substring(PreviousIndex));
                return;
            }

            if (TextInfo.Tag == Tag.Bold && NestedTextInfos.Peek().Tag == Tag.Italic)
            {
                TextInfo.ToNoFormatting();
                TextInfo = NestedTextInfos.Pop();
                State = States.Pop();
                UnderscoreCounter = 0;
                return;
            }

            var length = TextEnded ? index - PreviousIndex + 1 : index - PreviousIndex;
            length -= UnderscoreCounter == 1 ? 1 : 2;
            TextInfo.AddText(Markdown.Substring(PreviousIndex, length));
            TextInfo = NestedTextInfos.Pop();
            State = States.Pop();
            PreviousIndex = index;

            if (UnderscoreCounter > 2)
            {
                UnderscoreCounter -= 2;
                OpenUnderscoreTag(index);
            }
            else
            {
                UnderscoreCounter = 0;
            }
        }

        protected void ParseInsideWord(int index)
        {
            if (TextEnded || char.IsWhiteSpace(Markdown[index]))
            {
                if (underscoresCountInWord != 0)
                {
                    underscoresCountInWord = 0;
                    var length = TextEnded ? index - WordStartIndex + 1 : index - WordStartIndex;
                    var word = Markdown.Substring(WordStartIndex, length);
                    var previousStringLength = WordStartIndex - PreviousIndex;
                    if (previousStringLength > 0)
                        TextInfo.AddText(Markdown.Substring(PreviousIndex, previousStringLength));

                    UnderscoreCounter = CountUnderscoresAtTheWordEnd(word);
                    word = word.Substring(0, word.Length - UnderscoreCounter);
                    PreviousIndex = TextEnded ? index - UnderscoreCounter + 1 : index - UnderscoreCounter;
                    TextInfo.AddContent(WordParser.Parse(word));
                }

                State = States.Pop();
                State(index);
            }
            else
            {
                if (ShouldEscaped(Markdown[index]))
                    BackslashCounter = 0;
                else if (Markdown[index] == '_')
                    underscoresCountInWord++;
            }
            
        }

        private bool TagsAreIntersects()
        {
            return UnderscoreCounter == 1 && TextInfo.Tag != Tag.Italic ||
                   UnderscoreCounter != 1 && TextInfo.Tag != Tag.Bold;
        }
        
        private bool HasOpeningTag()
        {
            var result = false;
            var tag = UnderscoreCounter == 1 ? Tag.Italic : Tag.Bold;
            if (TextInfo.Tag == tag)
                return true;
            
            var temporaryStack = new Stack<TextInfo>();
            while (NestedTextInfos.Count != 0)
            {
                var textInfo = NestedTextInfos.Pop();
                if (textInfo.Tag == tag)
                {
                    NestedTextInfos.Push(textInfo);
                    result = true;
                    break;
                }
                temporaryStack.Push(textInfo);
            }

            while (temporaryStack.Count != 0)
                NestedTextInfos.Push(temporaryStack.Pop());

            return result;
        }

        private void ChangeAllConflictedTagsToNoFormatting(Tag currentTag, Tag conflictedTag)
        {
            TextInfo.ToNoFormatting();
            State = States.Pop();

            var temporaryStack = new Stack<TextInfo>();
            TextInfo textInfo;
            do
            {
                textInfo = NestedTextInfos.Pop();
                if (textInfo.Tag != conflictedTag && textInfo.Tag != currentTag)
                    break;

                textInfo.ToNoFormatting();
                State = States.Pop();
                temporaryStack.Push(textInfo);

                if (currentTag == textInfo.Tag)
                   break;
            } while (currentTag != textInfo.Tag);

            TextInfo = textInfo;
        }

        private int CountUnderscoresAtTheWordEnd(string word)
        {
            var count = 0;
            for (var i = word.Length - 1; i > 0; i--)
            {
                if (word[i] == '\\')
                    BackslashCounter++;
                else if (word[i] == '_')
                    count++;
                else
                {
                    if (count != 0 && ShouldEscaped('_'))
                        count--;
                    return count;
                }
            }

            return count;
        }
    }
}