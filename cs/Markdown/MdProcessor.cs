using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.DTOs;
using Markdown.LanguageProcessors;
using Markdown.Tags;
using Markdown.Validators;
using Markdown.Wrappers;

namespace Markdown
{
    public class MdProcessor : ILanguageProcessor
    {
        public List<ITag> Tags { get; private set; }
        public Dictionary<string, bool> IsOpen { get; private set; } = new Dictionary<string, bool>();
        public Wrapper Wrapper { get; private set; }
        public Validator Validator { get; private set; }
        private readonly Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();

        public MdProcessor(List<ITag> listTags, Wrapper wrapper)
        {
            Tags = listTags.OrderByDescending(x => x.Length).ToList();
            foreach (var tag in listTags)
            {
                IsOpen[tag.StringTag] = false;
            }

            Wrapper = wrapper;
            Wrapper.SetDefaultMap();
            Validator = new MDValidator(IsOpen, Tags);
        }

        public string Render(string mdParagraph)
        {
            stackTags.Clear();
            var i = 0;
            var result = new StringBuilder(mdParagraph);
            while (i < result.Length)
            {
                ITag tag;
                if (Validator.TryGetTag(i, result.ToString(), out tag))
                {
                    if (Validator.IsEscaped(i, result.ToString()))
                    {
                        i += tag.Length;
                        continue;
                    }

                    TagTypeContainer typeContainer;
                    if (Validator.TryGetCorrectTagContainer(i, result.ToString(), out typeContainer, tag))
                    {
                        if (CheckNumbers(i, tag, result.ToString()))
                        {
                            i += tag.Length + 1;
                            continue;
                        }

                        if (typeContainer.TagClass == TagClassEnum.Closer)
                        {
                            if (stackTags.Peek().Tag.TagType == typeContainer.TagType)
                            {
                                WrapTag(typeContainer, result);
                                stackTags.Pop();
                                ChangeState(tag.StringTag);
                                i += tag.Length;
                                continue;
                            }
                        }

                        if (CheckNestingViolation(tag))
                        {
                            i += tag.Length;
                            continue;
                        }

                        stackTags.Push(typeContainer);
                        ChangeState(tag.StringTag);
                        i += tag.Length;
                    }
                    else i += tag.Length;
                }
                else i += 1;
            }

            return result.ToString();
        }

        private void WrapTag(TagTypeContainer typeContainer, StringBuilder result)
        {
            var token = GetToken(typeContainer, stackTags.Peek(), result.ToString());
            var wrappedTag =
                Wrapper.WrapWithTag(
                    typeContainer.Tag,
                    token);
            result.Remove(token.Start, token.End - token.Start + 1)
                .Insert(token.Start, wrappedTag);
        }

        private void ChangeState(string tag)
        {
            IsOpen[tag] = !IsOpen[tag];
        }

        private bool CheckNestingViolation(ITag tag)
        {
            if (tag.Parent != null && IsOpen[tag.Parent.StringTag])
            {
                return true;
            }

            return false;
        }

        private bool CheckNumbers(int i, ITag tag, string result)
        {
            if (i != 0)
            {
                int temp;
                if (i + tag.Length < result.Length)
                    if (int.TryParse(result[i - 1].ToString(), out temp) &&
                        int.TryParse(result[i + tag.Length].ToString(), out temp))
                    {
                        return true;
                    }
            }

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