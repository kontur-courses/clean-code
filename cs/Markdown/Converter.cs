using System;
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
            var tokens = tokenizer.Tokenize(text).ToArray();
            Array.Sort(tokens, (t1, t2) => t1.Start - t2.Start);
            var converted = new StringBuilder();
            var previousTokenEnd = -1;
            foreach (var tagToken in tokens)
            {
                converted.Append(text.Substring(previousTokenEnd + 1, tagToken.Start - previousTokenEnd - 1));
                converted.Append(resultTagStore.GetTag(tagToken.Type, tagToken.Role));
                previousTokenEnd = tagToken.End;
            }

            converted.Append(text[(previousTokenEnd + 1)..]);
            return converted.ToString();
        }
    }
}