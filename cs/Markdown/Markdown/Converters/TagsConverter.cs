using System.Collections.Generic;
using Markdown.MdTags;

namespace Markdown.Converters
{
    public static class TagsConverter
    {
        private const char EscapeChar = '\\';

        public static Stack<Tag> GetAllTags(string mdParagraph)
        {
            if (mdParagraph == null)
            {
                return new Stack<Tag>();
            }

            var slashesCount = 0;
            var openedTagsTypes = new List<TagType>();
            var result = new Stack<Tag>();
            var stack = new Stack<(TagType Type, int index)>();
            for (var i = 0; i <= mdParagraph.Length; i++)
            {
                if (i != mdParagraph.Length && mdParagraph[i].Equals(EscapeChar))
                {
                    slashesCount++;
                }

                var typeOfTag = GetTagType(mdParagraph, i);
                switch (typeOfTag)
                {
                    case TagType.None when !mdParagraph[i].Equals(EscapeChar):
                        slashesCount = 0;
                        continue;
                    case TagType.None:
                        continue;
                    case TagType.StrongText:
                        i++;
                        break;
                }

                if (slashesCount % 2 == 1)
                {
                    continue;
                }

                if (!openedTagsTypes.Contains(typeOfTag) || stack.Count == 0)
                {
                    if (!IsCorrectOpening(mdParagraph, i, typeOfTag)) continue;
                    openedTagsTypes.Add(typeOfTag);
                    stack.Push((typeOfTag, i));
                }
                else
                {
                    var previousTags = new Stack<(TagType Type, int index)>();
                    previousTags.Push((typeOfTag, i));
                    if (!IsCorrectClosing(mdParagraph, i, typeOfTag)) continue;
                    while (stack.Count != 0 && previousTags.Count != 0)
                    {
                        if (stack.Peek().Type == previousTags.Peek().Type)
                        {
                            openedTagsTypes.Remove(stack.Peek().Type);
                            result.Push(TagBuilder.OfType(stack.Peek().Type)
                                .WithBounds(stack.Pop().index, previousTags.Pop().index));
                        }
                        else
                        {
                            previousTags.Push(stack.Pop());
                        }
                    }
                }
            }

            return new Stack<Tag>(GetAllCorrectTags(mdParagraph, result)); // разворот стека в обратном порядке
        }

        private static Stack<Tag> GetAllCorrectTags(string paragraph, Stack<Tag> tags)
        {
            var result = new Stack<Tag>();
            foreach (var tag in tags)
            {
                var isCorrect = true;
                if (tag.Type is TagType.Italics or TagType.StrongText)
                {
                    for (var i = tag.Start; i < tag.End; i++)
                    {
                        if (paragraph[i].Equals(' '))
                        {
                            isCorrect = false;
                            break;
                        }
                    }

                    if ((tag.Start == 0 || paragraph[tag.Start - 1].Equals(' ')) &&
                        (tag.End == paragraph.Length - 1 || paragraph[tag.End + 1].Equals(' ')))
                    {
                        isCorrect = true;
                    }

                    if (tag.Type == TagType.Italics && tag.End - tag.Start == 1 ||
                        tag.Type == TagType.StrongText && tag.End - tag.Start == 3)
                    {
                        isCorrect = false;
                    }
                }

                if (isCorrect)
                {
                    result.Push(tag);
                }
            }

            return result;
        }

        private static TagType GetTagType(string text, int index)
        {
            if (index == text.Length)
            {
                return TagType.Title;
            }

            var digits = new List<char>()
            {
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            var symbol = text[index];
            return symbol switch
            {
                '_' when index != 0 && digits.Contains(text[index - 1]) ||
                         index != text.Length - 1 && digits.Contains(text[index + 1]) => TagType.None,
                '_' when (index != text.Length - 1 && text[index + 1].Equals('_')) => TagType.StrongText,
                '_' => TagType.Italics,
                '*' => TagType.UnnumberedList,
                '+' => TagType.ListElement,
                '#' when index == 0 => TagType.Title,
                _ => TagType.None
            };
        }

        private static bool IsCorrectOpening(string text, int index, TagType type)
        {
            return type switch
            {
                TagType.Italics when index != text.Length - 1 && text[index + 1].Equals(' ') => false,
                TagType.Italics => true,
                TagType.StrongText when index < text.Length - 1 && text[index + 1].Equals(' ') => false,
                _ => true
            };
        }

        private static bool IsCorrectClosing(string text, int index, TagType type)
        {
            return type switch
            {
                TagType.Italics when index != 0 && text[index - 1].Equals(' ') => false,
                TagType.Italics => true,
                TagType.StrongText when index != 1 && text[index - 2].Equals(' ') => false,
                _ => true
            };
        }
    }
}