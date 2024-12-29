using Markdown.Tags.ContextRules;

namespace Markdown.Tags
{
    public abstract class PairTag: Tag
    {
        protected List<IContextRule> Rules = [];
        protected bool isClose = false;

        protected PairTag(string markdownText, int tagStart): base(markdownText, tagStart)
        {
            MarkdownText = markdownText;
            Rules = [new UnderscoreTagRule(), new PairTagSelectPartWordRule()];
        }
        public override void TryCloseTag(int contextEnd, string sourceMdText, out int tagEnd, List<Tag>? nested = null)
        {
            tagEnd = contextEnd + MdTag.Length;
            if (isClose)
            {
                base.TryCloseTag(contextEnd, sourceMdText, out tagEnd, nested);
            }
        }
        public override bool AcceptIfContextEnd(int currentPosition)
        {
            var contextStart = TagStart + MdTag.Length;
            isClose = Rules.Any(rule => rule.IsContextEnd(MarkdownText.AsSpan().Slice(contextStart), currentPosition - contextStart, MdTag));
            return isClose;
        }

        public override bool AcceptIfContextCorrect(int currentPosition)
        {
            var contextStart = TagStart + MdTag.Length;
            if (!char.IsLetter(MarkdownText[currentPosition]) && (TagStart == 0 || !char.IsLetter(MarkdownText[TagStart - 1]))) 
            {
                Rules = [ new UnderscoreTagRule(), new PairTagSelectFewWordsRule()];
            };

            return Rules.All(rule => rule.IsContextCorrect(MarkdownText.AsSpan().Slice(contextStart), currentPosition - contextStart, MdTag));
        }
    }
}
