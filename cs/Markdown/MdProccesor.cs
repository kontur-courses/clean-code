//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//namespace Markdown
//{
//    public class MdProccesor
//    {
//        private Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();
//
//        private bool isOpenOne;
//        private bool isOpenTwo;
//
//        private List<Tag> tags = new List<Tag>();
//
//        public MdProccesor(List<Tag> list)
//        {
//            this.tags = list.OrderByDescending(x => x.Length).ToList();
//        }
//
//        public String Render(string mdParagraph)
//        {
//            stackTags.Clear();
//            var i = 0;
//            var result = new StringBuilder(mdParagraph);
//            while (i < result.Length)
//            {
//                if (IsEscapingSymbol(i, result.ToString()))
//                {
//                    i += 2;
//                    continue;
//                }
//
//                Tag tag2;
//                if (CheckIsTagSymbol(i, result.ToString(), out tag2))
//                {
//                    TagTypeContainer tag;
//                    if (IsCorrectSpacedTag(i, result.ToString(), out tag, tag2))
//                    {
//                        if (i != 0)
//                        {
//                            int temp;
//                            switch (tag.TagType)
//                            {
//                                case TagTypeEnum.TwoUnderScore:
//                                    if (i + 2 < result.Length)
//                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
//                                            int.TryParse(result[i + 2].ToString(), out temp))
//                                        {
//                                            i += 3;
//                                            continue;
//                                        }
//
//                                    break;
//                                case TagTypeEnum.OneUnderscore:
//                                    if (i + 1 < result.Length)
//                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
//                                            int.TryParse(result[i + 1].ToString(), out temp))
//                                        {
//                                            i += 2;
//                                            continue;
//                                        }
//
//                                    break;
//                            }
//                        }
//
//                        if (tag.TagClass == TagClassEnum.Closer)
//                        {
//                            if (stackTags.Peek().TagType == tag.TagType)
//                            {
//                                var end = tag.TagType == TagTypeEnum.TwoUnderScore
//                                    ? tag.position + 1
//                                    : tag.position;
//                                var start = stackTags.Peek().TagType == TagTypeEnum.TwoUnderScore
//                                    ? stackTags.Peek().position + 2
//                                    : stackTags.Peek().position + 1;
//                                var token = new Token(result.ToString().Substring(start, tag.position - start),
//                                    stackTags.Peek().position, end);
//                                var wrappedTag =
//                                    HTMLWrapper.WrapWithTag(
//                                        tag.TagType == TagTypeEnum.TwoUnderScore ? "strong" : "em",
//                                        token.Value);
//                                result.Remove(token.Start, token.End - token.Start + 1)
//                                    .Insert(token.Start, wrappedTag);
//                                stackTags.Pop();
//                                isOpenOne = tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
//                                isOpenTwo = tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
//                                i = tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
//                                continue;
//                            }
//                        }
//
//                        if (tag.TagType == TagTypeEnum.TwoUnderScore && isOpenOne)
//                        {
//                            i += 2;
//                            continue;
//                        }
//
//                        stackTags.Push(tag);
//                        isOpenOne = tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
//                        isOpenTwo = tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
//                        i = tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
//                    }
//                    else i += 1;
//                }
//                else i += 1;
//            }
//
//            return result.ToString();
//        }
//
//        private bool CheckIsTagSymbol(int start, string input, out Tag tag2)
//        {
//            foreach (var tag in tags)
//            {
//                var k = 0;
//
//                if (tag.Length + start >= input.Length)
//                    continue;
//
//                for (var i = 0; i < tag.Length; i++)
//                {
//                    if (input[start + k] == tag.StringRepr[i])
//                    {
//                        k += 1;
//                    }
//                    else
//                    {
//                        break;
//                    }
//                }
//
//                if (k == tag.Length - 1)
//                {
//                    tag2 = tag;
//                    return true;
//                }
//            }
//
//            tag2 = null;
//            return false;
//        }
//
//
//        private bool IsEscapingSymbol(int ind, string inp)
//        {
//            if (inp[ind] == '\\')
//            {
//                return true;
//            }
//
//            return false;
//        }
//
//        private bool IsCorrectSpacedTag(int ind, string input, out TagTypeContainer tag, Tag tag2)
//        {
//            if (ind != 0)
//            {
//                if (ind + tag2.Length < input.Length) // "__ "
//                {
//                    if (Tag.isOpen)
//                    {
//                        if (input[ind - 1] != ' ')
//                        {
//                            Tag.isOpen = !Tag.isOpen;
//                            tag = new TagTypeContainer(tag2); // b_ closer
//                            return true;
//                        }
//                    }
//
//                    if (input[ind + tag2.Length] != ' ')
//                    {
//                        tag = new TagTypeContainer(tag2); // opener 
//                        return true;
//                    }
//                }
//                
//                if (ind + tag2.Length == input.Length) //"__"
//                {
//                    if (tag2.isOpen)
//                    {
//                        if (input[ind - 1] != ' ')
//                        {
//                            tag = new TagTypeContainer(tag2); // "b_" closer
//                            return true;
//                        }
//                    }
//                    
//                    tag = new TagTypeContainer(tag2); //opener
//                    return true;
//                }
//            }
//
//            if (input[ind + tag2.Length] != ' ')
//            {
//                tag = new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind); // opene "_abc"
//                return true;
//            }
//
//            tag = null;
//            return false;
//        }
//    }
//}