using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdProccesor
    {
        private Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();
        private List<ITag> tags = new List<ITag>();
        private Dictionary<string, bool> isOpen = new Dictionary<string, bool>();
        private MdToHtmlWrapper htmlWrapper;

        public MdProccesor(List<ITag> list)
        {
            this.tags = list.OrderByDescending(x => x.Length).ToList();
            foreach (var tag in list)
            {
                isOpen[tag.StringTag] = false;
            }

            var dict = MdToHtmlWrapper.getMdToHtmlDefaultMap();
            htmlWrapper = new MdToHtmlWrapper(dict);
        }

        public String Render(string mdParagraph)
        {
            stackTags.Clear();
            var i = 0;
            var result = new StringBuilder(mdParagraph);
            while (i < result.Length)
            {
                ITag tag2;
                if (CheckIsTagSymbol(i, result.ToString(), out tag2))
                {
                    if (IsEscapingSymbol(i, result.ToString()))
                    {
                        i += tag2.Length;
                        continue;
                    }

                    TagTypeContainer tag;
                    if (IsCorrectSpacedTag(i, result.ToString(), out tag, tag2))
                    {
                        if (i != 0)
                        {
                            int temp;
                            switch (tag.TypeEnum)
                            {
                                case TypeEnum.TwoUnderScoreMd:
                                    if (i + 2 < result.Length)
                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                            int.TryParse(result[i + 2].ToString(), out temp))
                                        {
                                            i += 3;
                                            continue;
                                        }

                                    break;
                                case TypeEnum.OneUnderscoreMd:
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

                        if (tag.ClassEnum == ClassEnum.Closer)
                        {
                            if (stackTags.Peek().Tag.TypeEnum == tag.TypeEnum)
                            {
                                var token = MdToHtmlWrapper.GetToken(tag, stackTags.Peek(), result.ToString());

                                var wrappedTag =
                                    htmlWrapper.WrapWithTag(
                                        tag.Tag,
                                        token);
                                result.Remove(token.Start, token.End - token.Start + 1)
                                    .Insert(token.Start, wrappedTag);
                                stackTags.Pop();
                                isOpen[tag2.StringTag] = !isOpen[tag2.StringTag];
                                i = tag.TypeEnum == TypeEnum.TwoUnderScoreMd ? i + 2 : i + 1;
                                continue;
                            }
                        }

                        if (tag.TypeEnum == TypeEnum.TwoUnderScoreMd && isOpen["_"])
                        {
                            i += 2;
                            continue;
                        }

                        stackTags.Push(tag);
                        isOpen[tag2.StringTag] = !(isOpen[tag2.StringTag]);
                        i = tag.TypeEnum == TypeEnum.TwoUnderScoreMd ? i + 2 : i + 1;
                    }
                    else i += tag2.Length;
                }
                else i += 1;
            }

            return result.ToString();
        }

        private bool CheckIsTagSymbol(int start, string input, out ITag tag2)
        {
            for (var j = 0; j < tags.Count; j++)
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
            if (ind - 1 >= 0)
                if (inp[ind - 1] == '\\')
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
                            return true;
                        }
                    }

                    if (input[ind + tag2.Length] != ' ')
                    {
                        tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
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
                            return true;
                        }
                    }

                    tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
                    return true;
                }
            }

            if (input[ind + tag2.Length] != ' ')
            {
                tag = new TagTypeContainer(tag2, isOpen[tag2.StringTag], ind); // opener
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