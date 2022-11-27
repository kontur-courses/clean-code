using Markdown.Core.Entities.Abstract;

namespace Markdown.Core.Entities.TagMaps
{
    public class StrongTag : BaseTagDelimiter
    {        
        protected override int DelimiterLength => 2;
        protected override int Priority => 1;
        protected override string Prefix => "<strong>";
        protected override string Postfix => "</strong>";
        public StrongTag()
        {
            Delimeters = new HashSet<string>(new[] { new string('*', DelimiterLength), new string('_', DelimiterLength) });
        }
    }
}
