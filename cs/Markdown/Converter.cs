using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Markdown.TagStore;
using Markdown.Tokens;

namespace Markdown
{
    public class Converter : IConverter
    {
        private readonly ITagStore resultTagStore;

        private readonly ITokenizer tokenizer;

        public Converter(ITagStore sourceTagStore, ITagStore resultTagStore)
        {
            this.resultTagStore = resultTagStore;
            tokenizer = new Tokenizer(sourceTagStore);
        }

        public string Convert(string text)
        {
            var converted = new StringBuilder();
            foreach (var token in TagTokenSpecifier.Normalize(tokenizer.Tokenize(text), text))
            {
                switch (token.TokenType)
                {
                    case TokenType.Text:
                        converted.Append(text.Substring(token.Start, token.Length));
                        break;
                    case TokenType.Tag:
                        var convertedTag = resultTagStore.GetTag(token.TagType, token.TagRole);
                        converted.Append(convertedTag);
                        break;
                    case TokenType.Escape:
                        converted.Append('\\');
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unidentified token type {token.TokenType.ToString()}");
                }
            }

            return converted.ToString();
        }
    }
}