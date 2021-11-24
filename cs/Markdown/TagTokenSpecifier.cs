using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown
{
    public static class TagTokenSpecifier
    {
        private static Dictionary<TagType, int> tagToPriority = new()
        {
            { TagType.Emphasized, 4 },
            { TagType.Strong, 3 },
            { TagType.Header, 1 }
        };


        public static IEnumerable<Token> Normalize(IEnumerable<Token> tokens, string text)
        {
            /*var a = ApplyPriority(tokens).ToArray();
            var b = a.ExcludeTagsWithNumbersAndWhiteSpaces(text).ToArray();*/
            
            return ApplyPriority(tokens).ExcludeTagsWithNumbersAndWhiteSpaces(text).OrderBy(t=>t.Start);
        }

        private static IEnumerable<Token> ApplyPriority(this IEnumerable<Token> tokens)
        {
            var priorityStack = new Stack<int>();
            priorityStack.Push(0);
            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Text)
                {
                    yield return token;
                    continue;
                }

                var currentPriority = priorityStack.Peek();
                if (tagToPriority[token.TagType] < currentPriority)
                {
                    token.SwitchToText();
                }
                else switch (token.TagRole)
                {
                    case TagRole.Closing:
                        priorityStack.Pop();
                        break;
                    case TagRole.Opening:
                        priorityStack.Push(tagToPriority[token.TagType]);
                        break;
                }

                yield return token;
            }
        }

        private static IEnumerable<Token> ExcludeTagsWithNumbersAndWhiteSpaces(this IEnumerable<Token> tokens, string text)
        {
            var openingTags = new Stack<Token>();
            foreach (var current in tokens)
            {
                if (current.TokenType == TokenType.Text)
                {
                    yield return current;
                    continue;
                }

                if (current.TokenType == TokenType.Tag)
                {
                    switch (current.TagRole)
                    {
                        case TagRole.Closing:
                            var opening = openingTags.Pop();
                            var textBetween = text.Substring(opening.End + 1, current.Start - opening.End - 1);
                            if (textBetween.Any(char.IsDigit) || textBetween.Length == 0)
                            {
                                opening.SwitchToText();
                                current.SwitchToText();
                            }

                            yield return opening;
                            yield return current;
                            break;
                        case TagRole.Opening:
                            openingTags.Push(current);
                            break;
                        default:
                            yield return current;
                            break;
                    }
                }
            }
        }
    }
}