using System.Collections.Generic;
using System.Text;
using Markdown.Tags;
using Markdown.TokenSystem;

namespace Markdown.Markup
{
    public class MarkupParser
    {
        private readonly Dictionary<string, Tag> supportedTags;

        public MarkupParser(Dictionary<string, Tag> supportedTags)
        {
            this.supportedTags = supportedTags;
        }

        public IEnumerable<Markup> ParseMarkup(string text)
        {
            var markupParts = new List<Markup>();
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var sign = text[i].ToString();
                var nextSign = i + 1 != text.Length ? text[i + 1].ToString() : string.Empty;
                Tag tagToParse;

                if (supportedTags.ContainsKey(sign + nextSign))
                    tagToParse = supportedTags[sign + nextSign];
                else if (supportedTags.ContainsKey(sign))
                    tagToParse = supportedTags[sign];
                else
                {
                    stringBuilder.Append(sign);
                    continue;
                }

                ParseTag(tagToParse, new TokenReader(text, i + tagToParse.Value.Length), 
                    stringBuilder, markupParts, out var offset);
                i += offset;
            }

            if (stringBuilder.Length > 0)
                markupParts.Add(new Markup(stringBuilder.ToString(), null));
            return markupParts;
        }

        private void ParseTag(Tag tagToParse, TokenReader tokenReader, StringBuilder stringBuilder, 
            List<Markup> markupParts, out int offset)
        {
            if (TryParseTagValue(tagToParse, tokenReader, out var markup))
            {
                if (stringBuilder.Length > 0)
                    RegisterMarkup(markupParts, stringBuilder, null);
                stringBuilder.Append(markup);
                RegisterMarkup(markupParts, stringBuilder, tagToParse);
                    
                offset = markup.Length + tagToParse.Value.Length * 2 - 1;
            }
            else if (tagToParse.Value.Length > 1)
            {
                stringBuilder.Append(tagToParse.Value);
                offset = tagToParse.Value.Length - 1;
            }
            else
            {
                stringBuilder.Append(tagToParse.Value);
                offset = 0;
            }
        }

        private void RegisterMarkup(List<Markup> markupParts, StringBuilder stringBuilder, Tag tag)
        {
            markupParts.Add(new Markup(stringBuilder.ToString(), tag));
            stringBuilder.Clear();
        }

        private bool TryParseTagValue(Tag tag, TokenReader tokenReader, out string markup)
        {
            if (tokenReader.Position >= tokenReader.Text.Length)
            {
                markup = string.Empty;
                return false;
            }

            var tagToken = tokenReader.ReadUntil(str => str == tag.CompletionSign);
            var tagEndFound = CheckTagEndFound(tag, tokenReader) && tagToken.Length > 0;
            markup = tagEndFound ? tagToken.Value : string.Empty;
            return tagEndFound;
        }

        private bool CheckTagEndFound(Tag tag, TokenReader tokenReader)
        {
            var text = tokenReader.Text;
            return (tokenReader.Position < text.Length 
                   && text[tokenReader.Position].ToString() == tag.CompletionSign)
                   || (tokenReader.Position + 1 < text.Length 
                       && text[tokenReader.Position].ToString() + text[tokenReader.Position + 1] ==
                       tag.CompletionSign);
        }
    }
}