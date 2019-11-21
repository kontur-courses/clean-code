using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDown.TagParsers;
using MarkDown.Tokens;

namespace MarkDown
{
    public static class TagParser
    {
        public static IEnumerable<MdToken> ParseTokensWithTag(IEnumerable<MdToken> tokens, Tag tag)
        {
            if (tokens == null || tag == null)
                throw new ArgumentException();
            var dividedTokens = TagTokenizer.DivideTokensByTag(tokens, tag).ToArray();
            for (var i = 0; i < dividedTokens.Length; i++)
            {
                if (dividedTokens[i].TokenType == TokenTypes.Tag &&
                    dividedTokens[i] is TagToken tagToken &&
                    tagToken.Tag.Equals(tag))
                {
                    if (IsSurroundedByDigits(dividedTokens, i))
                        tagToken.ToStringToken();
                    yield return tagToken;
                }
                else
                    yield return dividedTokens[i];
            }
        }

        public static IEnumerable<MdToken> CorrectIntersectingTags(IEnumerable<MdToken> tokens)
        {
            var tagStack = new TraverseStack<TagToken>();
            var tokenArray = tokens.ToArray();
            var result = new List<MdToken>();
            foreach (var token in tokenArray)
            {
                if (token.TokenType == TokenTypes.Tag)
                {
                    var tagToken = token as TagToken;
                    if (tagStack.Contains(tagToken))
                    {
                        result.AddRange(CollectNestedTagsAndCloseTag(tagToken, tagStack));
                        tagStack.Remove(tagToken);
                    }
                    else
                    {
                        if (IsAllowedToBeNested(tagToken, tagStack))
                            tagStack.Push(tagToken);
                        else
                            tagToken.ToStringToken();
                        result.Add(tagToken);
                    }
                }
                else
                    result.Add(token);
            }
            ProcessUnpairedTokens(tagStack);
            return result;
        }

        public static string TokensToString(IEnumerable<MdToken> tokens)
        {
            var builder = new StringBuilder();
            var stack = new TraverseStack<TagToken>();
            foreach (var token in tokens)
            {
                if (token.TokenType == TokenTypes.String)
                    builder.Append(token.Value);
                else if (token.TokenType == TokenTypes.Tag)
                {
                    var tagToken = token as TagToken;
                    if (stack.Contains(tagToken))
                    {
                        stack.Remove(tagToken);
                        builder.Append(tagToken.Tag.ClosingHtmlTag);
                    }
                    else
                    {
                        stack.Push(tagToken);
                        builder.Append(tagToken.Tag.OpeningHtmlTag);
                    }
                }
            }
            return builder.ToString();
        }

        public static IEnumerable<MdToken> FindPairTags(IEnumerable<MdToken> tokens)
        {
            var stack = new TraverseStack<TagToken>();
            var tokenArray = tokens.ToArray();
            var result = new List<MdToken>();
            for (var index = 0; index < tokenArray.Length; index++)
            {
                var token = tokenArray[index];
                if (token.TokenType == TokenTypes.Tag)
                {
                    var tagToken = (TagToken) token;
                    if (!stack.Contains(tagToken))
                    {
                        if (IsWhitespaceAhead(tokenArray, index))
                            tagToken.ToStringToken();
                        else
                            stack.Push(tagToken);
                    }
                    else
                    {
                        if(IsWhitespaceBehind(tokenArray, index) ||
                           IsCreatingZeroLengthTag(tokenArray, index, tagToken))
                            tagToken.ToStringToken();
                        else
                            stack.Remove(tagToken);
                    }
                }
                result.Add(token);
            }
            ProcessUnpairedTokens(stack);
            return result;
        }

        public static IEnumerable<MdToken> Tokenize(string line) //Weird decision but it is easier to use. Don't know how to replace
        {
            yield return new StringToken(line, line.Length);
        }

        private static bool IsWhitespaceAhead(MdToken[] tokens, int index)
        {
            return index + 1 < tokens.Length && char.IsWhiteSpace(tokens[index + 1].Value[0]);
        }

        private static bool IsWhitespaceBehind(MdToken[] tokens, int index)
        {
            return index != 0 && char.IsWhiteSpace(tokens[index - 1].Value.Last());
        }

        private static bool IsSurroundedByDigits(MdToken[] tokens, int index)
        {
            if (index == 0 || index == tokens.Length - 1)
                return false;
            return char.IsDigit(tokens[index - 1].Value.Last())
                   && char.IsDigit(tokens[index + 1].Value.Last());
        }

        private static bool IsCreatingZeroLengthTag(MdToken[] tokens, int index, TagToken currentToken)
        {
            return index > 0 && tokens[index - 1].TokenType == TokenTypes.Tag &&
                   currentToken.Tag.Equals((tokens[index - 1] as TagToken).Tag);
        }

        private static bool IsAllowedToBeNested(TagToken tag, TraverseStack<TagToken> tagParserStack) //Needs to be rewritten somehow
        {
            return !tag.Tag.Equals(new StrongTag()) || !tagParserStack.Contains(new TagToken(new EmTag()));
        }

        private static IEnumerable<TagToken> CollectNestedTagsAndCloseTag(TagToken currentToken,
            TraverseStack<TagToken> tagTokenStack)
        {
            foreach (var tagToken in tagTokenStack.TraverseToElementAndReturnBack(currentToken).ToList())
                yield return tagToken;
        }

        private static void ProcessUnpairedTokens(TraverseStack<TagToken> stack)
        {
            foreach (var unpairedToken in stack)
                unpairedToken.ToStringToken();
        }
    }
}