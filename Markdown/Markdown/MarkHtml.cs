namespace Markdown
{
    public class MarkHtml : MarkTranslatorElement
    {
        public MarkHtml(string mark, string tag) : base(mark,tag,tag.Insert(1,"/")){}
    }
}