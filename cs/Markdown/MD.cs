using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
    public class MD
    {
        private Stack<TagTypeContainer> stackTags;

        private bool isOpenOne;
        private bool isOpenTwo;

        public String Render(string mdParagraph)
        {
            stackTags = new Stack<TagTypeContainer>();
            var i = 0;
            var result = new StringBuilder(mdParagraph);
            while (i < result.Length)
            {
                if (CheckIsTagSymbol(i, result.ToString()))
                {
                    var tag = CheckIsCorrectTag(i, result.ToString());
                    if (i != 0)
                    {
                        if (tag.TagType == TagTypeEnum.TwoUnderScore)
                        {
                            if (result[i - 1] == '\\')
                            {
                                i += 2;
                                continue;
                            }
                        }
                        else if (result[i - 1] == '\\')
                        {
                            i += 1;
                            continue;
                        }
                        
                        if (i + 1 < result.Length)
                        {
                            if (tag.TagType == TagTypeEnum.TwoUnderScore)
                            {
                                try
                                {
                                    int.Parse(result[i - 1].ToString());
                                    int.Parse(result[i + 2].ToString());
                                    i += 3;
                                    continue;
                                }
                                catch(Exception e)
                                {
                                    
                                }
                            }
                            if (tag.TagType == TagTypeEnum.OneUnderscore)
                            {
                                try
                                {
                                    int.Parse(result[i - 1].ToString());
                                    int.Parse(result[i + 1].ToString());
                                    i += 2;
                                    continue;
                                }
                                catch(Exception e)
                                {
                                    
                                }
                            }
                        }
                    }

                    if (null != (object) tag)
                    {
                        if (tag.TagClass == TagClassEnum.Closer)
                        {
                            if (stackTags.Peek().TagType == tag.TagType)
                            {
                                //HTMLWrapper(tag);
                                var end = tag.TagType == TagTypeEnum.TwoUnderScore ? tag.position + 1 : tag.position;
                                var start = stackTags.Peek().TagType == TagTypeEnum.TwoUnderScore
                                    ? stackTags.Peek().position + 2
                                    : stackTags.Peek().position + 1;
                                var token = new Token(result.ToString().Substring(start, tag.position - start),
                                    stackTags.Peek().position, end);
                                var wrappedTag =
                                    HTMLWrapper.WrapWithTag(tag.TagType == TagTypeEnum.TwoUnderScore ? "strong" : "em",
                                        token.Value);
                                result.Remove(token.Start, token.End - token.Start + 1).Insert(token.Start, wrappedTag);
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
                    else i += 2;
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


        private TagTypeContainer CheckIsCorrectTag(int ind, string input)
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
                                return new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Closer, ind);
                        }

                        if (ind + 2 < input.Length)
                            if (input[ind + 2] != ' ')
                            {
                                return new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
                            }
                        
                        if (ind + 2 == input.Length)
                            if (input[ind + 1] == '_')
                            {
                                return new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
                            }
                    }

                    if (input[ind] == '_' && input[ind + 1] != '_')
                    {
                        if (isOpenOne)
                        {
                            if (input[ind - 1] != ' ')
                                return new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Closer, ind);
                        }
                        else if (input[ind + 1] != ' ')
                            return new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
                    }
                }

                if (isOpenOne)
                    if (input[ind] == '_' && input[ind - 1] != ' ')
                        return new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Closer, ind);
                if (input[ind] == '_')
                    return new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
            }

            if (input[ind] == '_' && input[ind + 1] == '_' && input[ind + 2] != ' ')
                return new TagTypeContainer(TagTypeEnum.TwoUnderScore, TagClassEnum.Opener, ind);
            else if (input[ind] == '_' && input[ind + 1] != ' ' && input[ind + 1] != '_')
                return new TagTypeContainer(TagTypeEnum.OneUnderscore, TagClassEnum.Opener, ind);
            return null;
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