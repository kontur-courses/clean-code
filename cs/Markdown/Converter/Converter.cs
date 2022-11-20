using Markdown.Primitives;
using Markdown.Primitives.TokenHelper;

namespace Markdown.Converter
{
    public class Converter
    {
        /// <summary> Преобрвзование текста Markdown к тексту Html</returns>
        public string Convert(string text)
        {
            var tokens = TokenHelper.FindTokens(text);
            var tags = ConvertTokensToTags(tokens);
            var resultInHtml = BuildTextInHtml(text, tags);

            return "";
        }

        private IEnumerable<Tag> ConvertTokensToTags(IEnumerable<Token> tokenStorage)
        {
            throw new NotImplementedException();
        }

        private object BuildTextInHtml(string text, IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }
    }
}
