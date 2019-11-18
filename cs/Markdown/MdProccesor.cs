using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdProccesor
    {
        private Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();

        private bool isOpenOne;
        private bool isOpenTwo;

        private List<ITag> tags = new List<ITag>();
        private Dictionary<string, bool> isOpen = new Dictionary<string, bool>();
        
        public MdProccesor(List<ITag> list)
        {
            this.tags = list.OrderByDescending(x => x.Length).ToList();
            foreach (var tag in list)
            {
                isOpen[tag.StringTag] = false;
            }
        }

        public String Render(string mdParagraph)
        {
            stackTags.Clear();
            var i = 0;
            var result = new StringBuilder(mdParagraph);
            while (i < result.Length)
            {
                if (IsEscapingSymbol(i, result.ToString()))
                {
                    i += 2;
                    continue;
                }

                ITag tag2;
                if (CheckIsTagSymbol(i, result.ToString(), out tag2))
                {
                    TagTypeContainer tag;
                    if (IsCorrectSpacedTag(i, result.ToString(), out tag, tag2))
                    {
                        if (i != 0)
                        {
                            int temp;
                            switch (tag.Tag.TagType)
                            {
                                case TagTypeEnum.TwoUnderScore:
                                    if (i + 2 < result.Length)
                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                            int.TryParse(result[i + 2].ToString(), out temp))
                                        {
                                            i += 3;
                                            continue;
                                        }

                                    break;
                                case TagTypeEnum.OneUnderscore:
                                    if (i + 1 < result.Length)
                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                            int.TryParse(result[i + 1].ToString(), out temp))
                                        {
                                            i += 2;
                                            continue;
                                        }

                                    break;
                            }
                        }

                        if (tag.Tag.TagClass == TagClassEnum.Closer)
                        {
                            if (stackTags.Peek().Tag.TagType == tag.Tag.TagType)
                            {
                                var end = tag.Tag.TagType == TagTypeEnum.TwoUnderScore
                                    ? tag.position + 1
                                    : tag.position;
                                var start = stackTags.Peek().Tag.TagType == TagTypeEnum.TwoUnderScore
                                    ? stackTags.Peek().position + 2
                                    : stackTags.Peek().position + 1;
                                var token = new Token(result.ToString().Substring(start, tag.position - start),
                                    stackTags.Peek().position, end);
                                var wrappedTag =
                                    HTMLWrapper.WrapWithTag(
                                        tag.Tag.TagType == TagTypeEnum.TwoUnderScore ? "strong" : "em",
                                        token.Value);
                                result.Remove(token.Start, token.End - token.Start + 1)
                                    .Insert(token.Start, wrappedTag);
                                stackTags.Pop();
                                isOpenOne = tag.Tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
                                isOpenTwo = tag.Tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
                                i = tag.Tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
                                continue;
                            }
                        }

                        if (tag.Tag.TagType == TagTypeEnum.TwoUnderScore && isOpenOne)
                        {
                            i += 2;
                            continue;
                        }

                        stackTags.Push(tag);
                        isOpenOne = tag.Tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
                        isOpenTwo = tag.Tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
                        i = tag.Tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
                    }
                    else i += 1;
                }
                else i += 1;
            }

            return result.ToString();
        }

        private bool CheckIsTagSymbol(int start, string input, out ITag tag2)
        {
            for(var j =0; j<tags.Count; j++)
            {
                var tag = tags[j];
                var k = 0;

                if (tag.Length + start > input.Length)
                    continue;

                for (var i = 0; i < tag.Length; i++)
                {
                    if (input[start + k] == tag.StringTag[i])
                    {
                        k += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (k == tag.Length)
                {
                    tag2 = tag;
                    return true;
                }
            }

            tag2 = null;
            return false;
        }


        private bool IsEscapingSymbol(int ind, string inp)
        {
            if (inp[ind] == '\\')
            {
                return true;
            }

            return false;
        }

        private bool IsCorrectSpacedTag(int ind, string input, out TagTypeContainer tag, ITag tag2)
        {
            if (ind != 0)
            {
                if (ind + tag2.Length < input.Length) // "__ "
                {
                    if (isOpen[tag2.StringTag])
                    {
                        if (input[ind - 1] != ' ')
                        {
                            tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // b_ closer
                            isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                            return true;
                        }
                    }

                    if (input[ind + tag2.Length] != ' ')
                    {
                        tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
                        isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                        return true;
                    }
                }
                
                if (ind + tag2.Length == input.Length) //"__"
                {
                    if (isOpen[tag2.StringTag])
                    {
                        if (input[ind - 1] != ' ')
                        {
                            tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // closer
                            isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                            return true;
                        }
                    }
                    
                    tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
                    isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                    return true;
                }
            }

            if (input[ind + tag2.Length] != ' ')
            {
                tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
                isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                return true;
            }

            tag = null;
            return false;
        }

        public static void Main()
        {
            
        }
    }
}