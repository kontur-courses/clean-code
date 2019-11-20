using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdProccesor
    {
        private readonly Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();
        private List<ITag> tags;
        private readonly Dictionary<string, bool> isOpen = new Dictionary<string, bool>();
        private readonly Wrapper MDToWrapper;

        public MdProccesor(List<ITag> list, Wrapper wrapper)
        {
            this.tags = list.OrderByDescending(x => x.Length).ToList();
            foreach (var tag in list)
            {
                isOpen[tag.StringTag] = false;
            }

            MDToWrapper = wrapper;
            MDToWrapper.SetDefaultMap();
        }

        public String Render(string mdParagraph)
        {
            stackTags.Clear();
            var i = 0;
            var result = new StringBuilder(mdParagraph);
            while (i < result.Length)
            {
                ITag tag;
                if (TryGetTag(i, result.ToString(), out tag))
                {
                    if (IsEscapedSymbol(i, result.ToString()))
                    {
                        i += tag.Length;
                        continue;
                    }

                    TagTypeContainer typeContainer;
                    if (TryGetCorrectSpacedTagContainer(i, result.ToString(), out typeContainer, tag))
                    {
                        if (i != 0)
                        {
                            int temp;
                            if (i + typeContainer.Length < result.Length)
                                if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                    int.TryParse(result[i + typeContainer.Length].ToString(), out temp))
                                {
                                    i += typeContainer.Length + 1;
                                    continue;
                                }
                        }

                        if (typeContainer.TagClass == TagClassEnum.Closer)
                        {
                            if (stackTags.Peek().Tag.TagType == typeContainer.TagType)
                            {
                                var token = GetToken(typeContainer, stackTags.Peek(), result.ToString());

                                var wrappedTag =
                                    MDToWrapper.WrapWithTag(
                                        typeContainer.Tag,
                                        token);
                                result.Remove(token.Start, token.End - token.Start + 1)
                                    .Insert(token.Start, wrappedTag);
                                stackTags.Pop();
                                isOpen[tag.StringTag] = !isOpen[tag.StringTag];
                                i = typeContainer.TagType == TagTypeEnum.TwoUnderScoreMd ? i + 2 : i + 1;
                                continue;
                            }
                        }

                        if (typeContainer.TagType == TagTypeEnum.TwoUnderScoreMd && isOpen["_"])
                        {
                            i += 2;
                            continue;
                        }

                        stackTags.Push(typeContainer);
                        isOpen[tag.StringTag] = !(isOpen[tag.StringTag]);
                        i = typeContainer.TagType == TagTypeEnum.TwoUnderScoreMd ? i + 2 : i + 1;
                    }
                    else i += tag.Length;
                }
                else i += 1;
            }

            return result.ToString();
        }

        private bool TryGetTag(int start, string input, out ITag tag2)
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
                        k += 1;
                    else
                        break;
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


        private bool IsEscapedSymbol(int ind, string input)
        {
            return ind - 1 >= 0 ? input[ind - 1] == '\\' : false;
        }

        private bool TryGetCorrectSpacedTagContainer(int ind, string input, out TagTypeContainer tagContainer, ITag tag)
        {
            if (ind != 0)
            {
                if (ind + tag.Length < input.Length) // "__ "
                {
                    if (isOpen[tag.StringTag])
                    {
                        if (input[ind - 1] != ' ')
                        {
                            tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], ind); // b_ closer
                            return true;
                        }
                    }

                    if (input[ind + tag.Length] != ' ')
                    {
                        tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], ind); // opener
                        return true;
                    }
                }

                if (ind + tag.Length == input.Length) //"__"
                {
                    if (isOpen[tag.StringTag])
                    {
                        if (input[ind - 1] != ' ')
                        {
                            tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], ind); // closer
                            return true;
                        }
                    }

                    tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], ind); // opener
                    return true;
                }
            }

            if (input[ind + tag.Length] != ' ')
            {
                tagContainer = new TagTypeContainer(tag, isOpen[tag.StringTag], ind); // opener
                return true;
            }

            tagContainer = null;
            return false;
        }

        public static Token GetToken(TagTypeContainer closerTag, TagTypeContainer openerTag, string result)
        {
            var end = closerTag.position + closerTag.Tag.Length - 1;
            var stringValueStart = openerTag.position + openerTag.Tag.Length;
            var token = new Token(result.ToString().Substring(stringValueStart, closerTag.position - stringValueStart),
                openerTag.position, end);
            return token;
        }

        public static void Main()
        {
        }
    }
}