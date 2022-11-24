using Markdown.Core.Entities.Abstract;

namespace Markdown.Core.Entities.TagMaps
{
    internal class EmTag : BaseTagDelimiter
    {
        protected override int DelimiterLenght => 1;
        protected override int Priority => 0;
        protected override string Prefix => "<em>";
        protected override string Postfix => "</em>";

        public EmTag()
        {
            Delimeters = new HashSet<string>(new[] { new string('*', DelimiterLenght), new string('_', DelimiterLenght) });
        }
    }
}
