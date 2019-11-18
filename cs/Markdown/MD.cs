using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
    public class MD
    {
        private Stack<TagTypeContainer> stackTags = new Stack<TagTypeContainer>();

        private bool isOpenOne;
        private bool isOpenTwo;

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

                if (CheckIsTagSymbol(i, result.ToString()))
                {
                    TagTypeContainer tag;
                    if (IsCorrectSpacedTag(i, result.ToString(), out tag))
                    {
                        if (i != 0)
                        {
                            int temp;
                            switch (tag.TagType)
                            {
                                case TagTypeEnum.TwoUnderScore:
                                    if(i+2<result.Length)
                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                            int.TryParse(result[i + 2].ToString(), out temp))
                                        {
                                            i += 3;
                                            continue;
                                        }

                                    break;
                                case TagTypeEnum.OneUnderscore:
                                    if(i+1<result.Length)
                                        if (int.TryParse(result[i - 1].ToString(), out temp) &&
                                            int.TryParse(result[i + 1].ToString(), out temp))
                                        {
                                            i += 2;
                                            continue;
                                        }

                                    break;
                            }
                        }

                        if (tag.TagClass == TagClassEnum.Closer)
                        {
                            if (stackTags.Peek().TagType == tag.TagType)
                            {
                                var end = tag.TagType == TagTypeEnum.TwoUnderScore
                                    ? tag.position + 1
                                    : tag.position;
                                var start = stackTags.Peek().TagType == TagTypeEnum.TwoUnderScore
                                    ? stackTags.Peek().position + 2
                                    : stackTags.Peek().position + 1;
                                var token = new Token(result.ToString().Substring(start, tag.position - start),
                                    stackTags.Peek().position, end);
                                var wrappedTag =
                                    HTMLWrapper.WrapWithTag(
                                        tag.TagType == TagTypeEnum.TwoUnderScore ? "strong" : "em",
                                        token.Value);
                                result.Remove(token.Start, token.End - token.Start + 1)
                                    .Insert(token.Start, wrappedTag);
                                stackTags.Pop();
                                isOpenOne = tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
                                isOpenTwo = tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
                                i = tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
                                continue;
                            }
                        }

                        if (tag.TagType == TagTypeEnum.TwoUnderScore && isOpenOne)
                        {
                            i += 2;
                            continue;
                        }

                        stackTags.Push(tag);
                        isOpenOne = tag.TagType == TagTypeEnum.OneUnderscore ? !isOpenOne : isOpenOne;
                        isOpenTwo = tag.TagType == TagTypeEnum.TwoUnderScore ? !isOpenTwo : isOpenTwo;
                        i = tag.TagType == TagTypeEnum.TwoUnderScore ? i + 2 : i + 1;
                    }
                    else i += 1;
                }
                else i += 1;
            }

            return result.ToString();
        }

        private bool CheckIsTagSymbol(int start, string input)
        {
            if (input[start] == '_')
            {
                return true;
            }

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

        private bool IsCorrectSpacedTag(int ind, string input, out TagTypeContainer tag)
        {
            if (ind != 0)
            {
                if (ind + 1 < input.Length)
                {
                    if (input[ind + 1] == '_')
                    {
                        if (isOpenTwo)
                        {
                            if (input[ind - 1] != ' ')
                            {
                                tag = new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Closer, ind);
                                return true;
                            }
                        }

                        if (ind + 2 < input.Length)
                            if (input[ind + 2] != ' ')
                            {
                                tag = new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
                                return true;
                            }

                        if (ind + 2 == input.Length)
                            if (input[ind + 1] == '_')
                            {
                                tag = new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
                                return true;
                            }
                    }

                    if (input[ind] == '_' && input[ind + 1] != '_')
                    {
                        if (isOpenOne)
                        {
                            if (input[ind - 1] != ' ')
                            {
                                tag = new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Closer, ind);
                                return true;
                            }
                        }
                        else if (input[ind + 1] != ' ')
                        {
                            tag = new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
                            return true;
                        }
                    }
                }

                if (isOpenOne)
                    if (input[ind] == '_' && input[ind - 1] != ' ')
                    {
                        tag = new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Closer, ind);
                        return true;
                    }

                if (input[ind] == '_')
                {
                    tag = new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
                    return true;
                }
            }

            if (input[ind] == '_' && input[ind + 1] == '_' && input[ind + 2] != ' ')
            {
                tag = new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
                return true;
            }
            else if (input[ind] == '_' && input[ind + 1] != ' ' && input[ind + 1] != '_')
            {
                tag = new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
                return true;
            }

            tag = null;
            return false;
        }

        public static void Main(string[] args)
        {
            var test =
                //"_test_";
                //"_Test_ __ab__";
                //"__tag__";
                //"\\__not a tag__";
                //"_1_2_";
                "_1__2_";
            var md = new MD();
            var res = md.Render(test);
            Console.WriteLine(test);
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}