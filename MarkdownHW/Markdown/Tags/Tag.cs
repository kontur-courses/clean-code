using System.Collections.Generic;

namespace Markdown.Tokens
{
    public abstract class Tag
    {
        protected abstract string MdTag { get; }
        protected abstract string HtmlTag { get; }
        protected string OpenHtmlTag => $"<{HtmlTag}>";
        protected string CloseHtmlTag => $"</{HtmlTag}>";
        
        protected List<Tag> content;
        protected string text;
        protected int startsAt;
        protected int endsAt;
        
        public virtual string GetHtml => $"{OpenHtmlTag}{GetContent()}{CloseHtmlTag}";

        protected Tag(string text, int tagStartsAt)
        {
            
        }

        protected string GetContent()
        {
            return "";
        }
    }
}