using System.Collections.Generic;
using System.Linq;
using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Rules;
using Markdown.Tags;
using Markdown.Tags.Html;
using Markdown.Tags.Markdown;

namespace Markdown.Html
{
    public class HtmlTokenParser : ITokenParser
    {
        private readonly List<Token> tokens;

        private readonly char[] signs =
        {
            ',', '.', '!', '?', ' ', '—', '-', ':', '#'
        };

        private int Index { get; set; } = 0;

        public HtmlTokenParser()
        {
            tokens = new List<Token>();
        }

        public IEnumerable<Token> Parse(string data)
        {
            var dataLines = data.Split("\r\n");

            foreach (var line in dataLines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                
                Index = 0;
                var rootToken = GetRootToken(line);
                tokens.Add(ParseLine(rootToken, line));
            }

            return tokens;
        }

        private Token ParseLine(Token token, string line)
        {
            var tagString = string.Empty;
            while (Index < line.Length)
            {
                var symbol = line[Index];
                if (char.IsLetter(symbol) || char.IsDigit(symbol) || signs.Contains(symbol))
                {
                    if (CheckAndUpdateTokenIfInDifferentWords(symbol, token))
                        return null;
                    if (TryCreateHtmlTag(tagString, out var htmlTag) != null)
                    {
                        if (htmlTag.Equals(token.Tag))
                            return token;

                        AddTokenOrIgnoreIt(token, line, htmlTag, tagString);
                        
                        tagString = string.Empty;
                        continue;
                    }
                    if (TryAddTextToken(token, line)) 
                        continue;

                    if (IsNumberInHighlightingTag(symbol, token.Parent.Tag))
                        token.Parent.ToTextToken();
                    
                    token.Content += symbol;
                }
                else if (token.Type == TokenType.Text)
                    return token;
                else
                    tagString += symbol;
                Index += 1;
            }
            return ProcessWhenLineEnd(token, tagString, line);
        }

        private bool IsNumberInHighlightingTag(char symbol, ITag tag)
        {
            return char.IsDigit(symbol) && Tag.IsHighlightingTag(tag);
        }

        private void AddTokenOrIgnoreIt(Token token, string line, ITag htmlTag, string tagString)
        {
            if (IsNeedIgnoreTag(token.Tag, htmlTag))
                token.Childrens[^1].Content += tagString;
            else
                token.TryAddToken(ParseLine(new Token(TokenType.Nested, htmlTag, token), line));
        }

        private Token ProcessWhenLineEnd(Token token, string tagString, string line)
        {
            if (!string.IsNullOrEmpty(tagString))
            {
                if (TryCreateHtmlTag(tagString, out var tag) != null)
                    if (token.Tag.Equals(tag))
                        return token;
                token.Childrens.Add(ParseLine(new Token(TokenType.Text, Tag.Empty, token)
                {
                    Content = tagString
                }, line));
            }

            if (!Tag.IsHighlightingTag(token.Tag))
                return token;

            return token.Type != TokenType.Root ? token.ToTextToken() : token;
        }
        
        private bool TryAddTextToken(Token token, string line)
        {
            if (token.Type == TokenType.Text) return false;
            token.TryAddToken(ParseLine(new Token(TokenType.Text, Tag.Empty, token), line));
            return true;
        }

        private bool IsNeedIgnoreTag(ITag firstHtmlTag, ITag secondHtmlTag)
        {
            var firstMarkdownTag = MarkdownToHtml.Rules.TryGetKeyByValue(firstHtmlTag);
            var secondMarkdownTag = MarkdownToHtml.Rules.TryGetKeyByValue(secondHtmlTag);
            
            if (firstMarkdownTag != null && secondHtmlTag != null)
                return firstMarkdownTag.NeedTagIgnore(secondMarkdownTag);

            return false;
        }

        private ITag TryCreateHtmlTag(string tagString, out ITag tag)
        {
            MarkdownToHtml.Rules.TryGetValue(new Tag(tagString), out var value);
            tag = value;
            return tag;
        }

        private Token GetRootToken(string line)
        {
            var tag = new Tag(line[0].ToString());

            if (tag.Equals(MarkdownTags.Heading) && line[1] == ' ')
            {
                Index = 2;
                return new Token(TokenType.Root, HtmlTags.Heading, null);
            }
            
            return new Token(TokenType.Root, HtmlTags.LineBreak, null);
        }

        private bool CheckAndUpdateTokenIfInDifferentWords(char symbol, Token token)
        {
            if (!char.IsWhiteSpace(symbol)) return false;
            if (token.Parent == null || !Tag.IsHighlightingTag(token.Parent.Tag)) return false;

            token.Parent.Childrens.Add(token);
            token.Parent.ToTextToken();
            
            return true;
        }
    }
}