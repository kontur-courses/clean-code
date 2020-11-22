using System.Collections.Generic;
using System.Text;
using Markdown.Tags;
using Markdown.TokenSystem;

namespace Markdown.MarkupParser
{
    public class MarkupParser
    {
        private readonly Dictionary<string, Tag> supportedTags;
        private readonly HashSet<char> shieldedSigns;

        public MarkupParser(Dictionary<string, Tag> supportedTags, HashSet<char> shieldedSigns)
        {
            this.supportedTags = supportedTags;
            this.shieldedSigns = shieldedSigns;
        }

        public string ParseMarkup(string text)
        {
            if (text == null || text.Length < 2)
                return text;
            var tokenReader = new TokenReader(text);
            var markup = new StringBuilder();
            TryParseMarkup(tokenReader, markup, new StringBuilder(), new Stack<Tag>());
            return markup.ToString();
        }

        private bool TryParseMarkup(TokenReader tokenReader,
            StringBuilder markupResult, StringBuilder tagResult, Stack<Tag> openTags)
        {
            var text = tokenReader.Text;
            for (var i = tokenReader.Position; i < text.Length; i++)
            {
                tokenReader.SkipSigns(i - tokenReader.Position);
                var sign = text[i].ToString();
                var nextSign = i + 1 != text.Length ? text[i + 1].ToString() : string.Empty;
                var tagToParse = GetTagToParse(sign, nextSign, openTags.Count > 0 ? openTags.Peek() : null);

                if (openTags.Count > 0 && 
                    (tagToParse != null && tagToParse.TagType == openTags.Peek().TagType 
                     || sign == openTags.Peek().CompletionSign))
                    return TryProcessTagEnd(openTags.Peek(), tokenReader, markupResult, tagResult, openTags);

                if (tagToParse == null)
                {
                    ParseText(sign, i, tokenReader, tagResult);
                    i = tokenReader.Position;
                    continue;
                }
                
                if (CheckTagStartValidity(tagToParse, tokenReader))
                {
                    openTags.Push(tagToParse);
                    ParseTag(tagToParse, tokenReader, tagResult, openTags);
                }
                else
                {
                    tokenReader.SkipSigns(tagToParse.Value.Length);
                    tagResult.Append(tagToParse.Value);
                    i = tokenReader.Position - 1;
                    continue;
                }
                i = tokenReader.Position;
            }

            if (tagResult.Length > 0)
                markupResult.Append(tagResult);

            return false;
        }

        private bool TryProcessTagEnd(Tag parsingTag, TokenReader tokenReader, StringBuilder markupResult,
            StringBuilder tagResult, Stack<Tag> openTags)
        {
            if (tagResult.Length == 0)
            {
                tokenReader.SkipSigns(openTags.Peek().CompletionSign.Length - 1);
                markupResult.Append(openTags.Pop().CompletionSign);
                return false;
            }
            
            if (CheckTagEndValidity(parsingTag, tokenReader) && tagResult.Length > 0)
            {
                tokenReader.SkipSigns(parsingTag.CompletionSign.Length - 1);
                var res = tagResult.ToString();
                tagResult.Clear();
                tagResult.Append(Converter.MarkupConverter.ConvertTagToHtmlString(parsingTag, res));
                openTags.Pop();
                return true;
            }

            var isIntersected =
                (parsingTag.TagType == TagType.Italic || parsingTag.TagType == TagType.Boldface)
                && CheckIntersectUnderscoreTags(parsingTag, tokenReader);
            if (!isIntersected && tagResult.Length > 0)
            {
                tagResult.Append(parsingTag.CompletionSign);
                tokenReader.SkipSigns(parsingTag.CompletionSign.Length);
                markupResult.Append(tagResult);
                openTags.Pop();
                return false;
            }

            if (isIntersected)
            {
                openTags.Push(supportedTags["__"]);
                ParseTag(supportedTags["__"], tokenReader, tagResult, openTags);
                return true;
            }
            return false;
        }

        private void ParseText(string sign, int position, TokenReader tokenReader, StringBuilder tagResult)
        {
            if (sign == "\\")
            {
                ProcessShieldingSign(tokenReader, tagResult);
                if (tokenReader.Position - position == 1)
                {
                    tokenReader.SetPosition(tokenReader.Position - 1);
                    return;
                }

                tokenReader.SetPosition(tokenReader.Position - (tokenReader.Position - position + 1) % 2);
                return;
            }

            var textToken = tokenReader.ReadUntil(str => shieldedSigns.Contains(str[0]) || str[0] == '\n');
            if (position == tokenReader.Position)
            {
                tagResult.Append(sign);
                return;
            }

            tagResult.Append(textToken.Value);
        }
        
        private void ParseTag(Tag tagToParse, TokenReader tokenReader, StringBuilder tagResult, Stack<Tag> openTags)
        {
            var nextTagResult = new StringBuilder();
            if (!TryParseMarkup(tokenReader.SkipSigns(tagToParse.Value.Length), 
                tagResult, nextTagResult, openTags))
            {
                if (nextTagResult.Length > 0)
                    tagResult.Insert(tagResult.Length > 0
                            ? tagResult.Length - nextTagResult.Length
                            : 0,
                        tagToParse.Value);
                else
                    tagResult.Append(tagToParse.Value);
            }
            else tagResult.Append(nextTagResult);
        }

        private bool CheckIntersectUnderscoreTags(Tag tag, TokenReader tokenReader)
        {
            if (tag.TagType == TagType.Italic)
                return tokenReader.Position + 1 < tokenReader.Text.Length
                   && tokenReader.Text[tokenReader.Position + 1] == '_';
            return tokenReader.Position + 2 < tokenReader.Text.Length;
        }

        private bool CheckTagEndValidity(Tag tag, TokenReader tokenReader)
        {
            var text = tokenReader.Text;
            var position = tokenReader.Position;

            if (tag.TagType == TagType.Italic
                && (char.IsWhiteSpace(text[position - 1]) ||
                    (text[position - 1] == '_' && position - 2 > 0 && text[position - 2] != '\\')))
                return false;

            return tag.TagType == TagType.Header || !char.IsWhiteSpace(text[position - 1]);
        }

        private Tag GetTagToParse(string sign, string nextSign, Tag lastOpenTag)
        {
            Tag tagToParse = null;
            if (supportedTags.ContainsKey(sign + nextSign))
                tagToParse = supportedTags[sign + nextSign];
            else if (supportedTags.ContainsKey(sign))
                tagToParse = supportedTags[sign];

            if (tagToParse != null && lastOpenTag != null 
                                   && tagToParse.TagType == TagType.Boldface 
                                   && lastOpenTag.TagType == TagType.Italic)
                return supportedTags[sign];
            return tagToParse;
        }
        
        private bool CheckTagStartValidity(Tag tag, TokenReader tokenReader)
        {
            if (tag.TagType != TagType.Header
                && tokenReader.Position + tag.Value.Length < tokenReader.Text.Length
                && char.IsWhiteSpace(tokenReader.Text[tokenReader.Position + tag.Value.Length]))
                return false;

            if (tag.TagType == TagType.Header
                && tokenReader.Position - 1 > 0
                && tokenReader.Text[tokenReader.Position - 1] != '\n')
                return false;

            return true;
        }

        private void ProcessShieldingSign(TokenReader tokenReader, StringBuilder markupPart)
        {
            if (tokenReader.Position + 1 < tokenReader.Text.Length
                && shieldedSigns.Contains(tokenReader.Text[tokenReader.Position + 1]))
            {
                markupPart.Append(tokenReader.Text[tokenReader.Position + 1]);
                tokenReader.SkipSigns(2);
            }
            else
            {
                markupPart.Append("\\");
                tokenReader.SkipSigns(1);
            }
        }
    }
}