using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tokenizing
{
    public class Tokenizer
    {
        private readonly Language language;
        private readonly int maxTagLength;

        public Tokenizer(Language language)
        {
            this.language = language;
            maxTagLength = language.MaxTagLength;
        }

        public List<Token> Tokenize(string source)
        {
            var result = new List<Token>();
            var rawTagStart = 0;

            for (var i = 0; i < source.Length; i++)
            {
                if (TryParseToken(source.Substring(i, maxTagLength), out var token))
                {
                    result.Add(new Token(Tag.Raw, false, source.Substring(rawTagStart, i - rawTagStart)));
                    result.Add(token);

                    var tokenLength = GetTokenLength(token);
                    i += tokenLength;
                    rawTagStart = i;
                }
            }

            if (rawTagStart != source.Length)
                result.Add(new Token(Tag.Raw, false, source.Substring(rawTagStart, source.Length - rawTagStart)));

            return result;
        }

        public bool TryParseToken(string source, out Token token)
        {
            for (var length = source.Length; length > 0; length--)
            {
                if (language.TryParseOpeningTag(source.Substring(0, length), out var tag))
                {
                    token = new Token(tag, true);
                    return true;
                }

                if (language.TryParseClosingTag(source.Substring(0, length), out tag))
                {
                    token = new Token(tag, false);
                    return true;
                }
            }

            token = null;
            return false;
        }

        private int GetTokenLength(Token token)
        {
            return (token.IsOpening ? language.ConvertOpeningTag(token.Tag) : language.ConvertClosingTag(token.Tag))
                .Length;
        }
    }
}