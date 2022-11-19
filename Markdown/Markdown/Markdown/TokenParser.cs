using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public static class TokenParser
    {
        private static char[] digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        
        public static List<Token> GetTokens(string mdString)
        {

            var tokenList = MarkdownParser.GetArrayWithMdTags(mdString).OrderBy(tag => tag.Position).ToList();
            var tokens = new List<Token>();
            var firstToken = mdString
                .GetTextTokenBetweenTagTokens(new Token(0, 0), tokenList[0]);
            if (firstToken.Length > 0)
                tokens.Add(firstToken);
            var token = tokenList[0];
            for (var i = 0; i < tokenList.Count; i++)
            {
                if (AddTextTokens(mdString, i, tokenList, tokens))
                    continue;
                token = CreateToken(mdString, tokenList, i, tokens);
            }
            tokens.Add(mdString.GetTextTokenBetweenTagTokens(token, new Token(mdString.Length, 0)));
            tokens = tokens.RemoveFields().RemoveDigits(mdString).RemoveSingleTokens(mdString).RemoveStrongInItalics().ToList();
            return tokens;
        }

        private static Token CreateToken(string mdString, List<Token> tokenList, int i, List<Token> tokens)
        {
            var token = tokenList[i];
            token.Type = GetTokenType(mdString.Substring(token.Position, token.Length));
            token.Element = token.GetElementInText(mdString);
            tokens.Add(token);
            return token;
        }

        private static bool AddTextTokens(string mdString, int i, List<Token> tokenList, List<Token> tokens)
        {
            if (i >= 1)
            {
                var textToken = mdString.GetTextTokenBetweenTagTokens(tokenList[i - 1], tokenList[i]);
                if (textToken.Length > 0)
                    tokens.Add(textToken);
            }

            if (mdString[tokenList[i].Position] == '\\')
            {
                tokens.Add(new Token(tokenList[i].Position, tokenList[i].Length, TokenType.Field));
                return true;
            }

            return false;
        }

        public static IEnumerable<Token> RemoveSingleTokens(this IEnumerable<Token> tokens, string md)
        {
            var unknownTags = new Stack<Token>();
            var stackTokens = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default)
                {
                    foreach (var textToken in GetTextToken(md, token, unknownTags))
                        yield return textToken;
                    yield return token;
                    continue;
                }

                foreach (var tokenMd in EnumerableTokens(md, token, stackTokens, unknownTags))
                    yield return tokenMd;
            }

            foreach (var singleToken in ToStringTokensWithoutPair(stackTokens, unknownTags))
                yield return singleToken;

        }

        private static IEnumerable<Token> ToStringTokensWithoutPair(Stack<Token> stackTokens, Stack<Token> unknownTags)
        {
            foreach (var token in stackTokens.Concat(unknownTags))
            {
                token.ToDefault();
                yield return token;
            }
        }

        private static IEnumerable<Token> EnumerableTokens(string md, Token token, Stack<Token> stackTokens, Stack<Token> unknownTags)
        {
            if (token.Element == TokenElement.Open)
                stackTokens.Push(token);
            else if (token.Element == TokenElement.Close)
                foreach (var closeTag in stackTokens.CloseTags(unknownTags, token))
                    yield return closeTag;
            else if (token.Element == TokenElement.Unknown)
                foreach (var unknownTag in unknownTags.UnknownTags(md, stackTokens, token))
                    yield return unknownTag;
            else
                yield return token;
        }

        private static IEnumerable<Token> GetTextToken(string md, Token token, Stack<Token> unknownTags)
        {
            var mdStringBetweenTags = md.Substring(token.Position, token.Length);
            if (!mdStringBetweenTags.Contains(' '))
                yield break;
            foreach (var unknownTag in unknownTags)
            {
                unknownTag.ToDefault();
                yield return unknownTag;
            }
        }

        private static IEnumerable<Token> CloseTags(this Stack<Token> stackTags, Stack<Token> unknownStackTags,
            Token token)
        {
            if (stackTags.Count <= 0)
            {
                if (unknownStackTags.Count > 0 && unknownStackTags.Peek().Type == token.Type)
                {
                    unknownStackTags.Peek().Element = TokenElement.Open;
                    yield return unknownStackTags.Pop();
                    yield return token;
                    yield break;
                }
                token.ToDefault();
                yield return token;
                yield break;
            }

            var last = stackTags.Pop();
            if (last.Type != token.Type)
            {
                last.ToDefault();
                token.ToDefault();
            }

            yield return last;
            yield return token;
        }


        private static IEnumerable<Token> UnknownTags(this Stack<Token> undefinedStackTags, string md,
            Stack<Token> tagsStack, Token token)
        {
            if (undefinedStackTags.Count == 0)
            {
                if (tagsStack.Count > 0 && tagsStack.Peek().Type == token.Type)
                {
                    if (!md.Substring(tagsStack.Peek().End + 1,
                            token.Position - tagsStack.Peek().End - 1).Contains(' '))
                    {
                        token.Element = TokenElement.Close;
                        yield return tagsStack.Pop();
                        yield return token;
                    }
                    else
                        undefinedStackTags.Push(token);
                }
                else
                    undefinedStackTags.Push(token);
            }
            else if (undefinedStackTags.Peek().Type == token.Type)
            {
                undefinedStackTags.Peek().Element = TokenElement.Open;
                token.Element = TokenElement.Close;
                yield return undefinedStackTags.Pop();
                yield return token;
            }
            else
                undefinedStackTags.Push(token);
        }

        public static IEnumerable<Token> RemoveFields(this List<Token> tokens)
        {
            var field = false;
            var count = tokens.Count;
            for (var i = 0; i < count; i++)
            {
                var token = tokens[i];
                if (field)
                {
                    field = false;
                    token.ToDefault();
                    yield return token;
                    continue;
                }

                if (token.Type != TokenType.Field)
                {
                    yield return token;
                    continue;
                }
                token.ToDefault();
                if (i + 1 < count && tokens[i + 1].Type != TokenType.Default)
                    field = true;
            }
        }

        public static IEnumerable<Token> RemoveDigits(this IEnumerable<Token> tokens, string mdString)
        {
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default)
                {
                    yield return token;
                    continue;
                }
                if (!HaveDigit(mdString, token))
                {
                    yield return token;
                    continue;
                }
                token.ToDefault();
                yield return token;
            }
        }
        public static IEnumerable<Token> RemoveStrongInItalics(this IEnumerable<Token> tokens)
        {
            var lastOpenIsItalics = false;
            foreach (var token in tokens.OrderBy(x => x.Position))
            {
                if (token.Type == TokenType.Italic)
                {
                    if (token.Element == TokenElement.Close)
                        lastOpenIsItalics = false;
                    else
                    {
                        lastOpenIsItalics = true;
                    }
                }
                if (lastOpenIsItalics && token.Type == TokenType.Strong)
                {
                    token.ToDefault();
                    yield return token;
                    continue;
                }


                yield return token;
            }

        }
        private static bool HaveDigit(string MdString, Token token)
        {
            if (token.Position - 1 >= 0 && digits.Contains(MdString[token.Position - 1]))
                return true;
            if (token.Position + token.Length < MdString.Length &&
                digits.Contains(MdString[token.Position + token.Length]))
                return true;
            return false;
            //return ((token.Position - 1 > 0 && digits.Contains(MdString[token.Position - 1]))
            //        ||(token.Position + token.Length < MdString.Length && 
            //           digits.Contains(MdString[token.Position + token.Length]))) && false;
        }
        private static TokenType GetTokenType(string tag)
        {
            var type = TokenType.Default;
            type = tag switch
            {
                "# " => TokenType.Header,
                "__" => TokenType.Strong,
                "_" => TokenType.Italic,
                "\\" => TokenType.Field,
                _ => type
            };
            return type;
        }

    }
}

