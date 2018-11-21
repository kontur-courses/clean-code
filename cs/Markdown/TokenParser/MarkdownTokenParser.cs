using System.Collections.Generic;
using System.Linq;
using Markdown.Data;
using Markdown.TokenParser.TokenReaders;

namespace Markdown.TokenParser
{
    public class MarkdownTokenParser : ITokenParser
    {
        private readonly List<ITokenReader> tokenReaders = new List<ITokenReader>();

        public MarkdownTokenParser(IEnumerable<string> tags)
        {
            var tagsArray = tags as string[] ?? tags.ToArray();
            tokenReaders.Add(new EscapedTokenReader(tagsArray));
            tokenReaders.AddRange(tagsArray
                .OrderByDescending(tag => tag.Length)
                .Select(tag => new TagTokenParser(tag)));
            tokenReaders.Add(new SpaceTokenReader());
            tokenReaders.Add(new TextTokenReader());
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;
            var position = 0;
            while (position < text.Length)
            {
                var readerResult = tokenReaders
                    .Select(reader => reader.ReadToken(text, position))
                    .First(result => result.Success);
                position += readerResult.Shift;
                yield return readerResult.Token;
            }
        }
    }
}