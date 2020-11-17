using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class UlITagConverter : TagConverterBase
    {
        protected internal override bool IsSingleTag => false;

        protected override HashSet<string> TagInside => new HashSet<string>();

        protected override string Html => TagHtml.ul;

        protected override string Md => MarkdownElement.list;

        protected override bool CanClose(StringBuilder text, int pos) => CanCloseBase(text, pos);

        protected override bool CanOpen(StringBuilder text, int pos) => CanOpenBase(text, pos);

        protected internal override bool IsTag(string text, int pos) => true;

        public StringBuilder Convert(StringBuilder text)
        {
            var result = new StringBuilder();
            result.Append(StringMd);
            foreach(var tagText in Split(text, ';'))
            {
                result.Append(new LiITagConverter().FormTags(Markdown.Md.Render(tagText)));
            }
            result.Append(StringMd);
            return FormTags(result);
        }

        private IEnumerable<StringBuilder> Split(StringBuilder text, char simbol)
        {
            var curentText = new StringBuilder();
            for(var i = LengthMd; i < text.Length - LengthMd; i++)
            {
                if(text[i] == Markdown.Md.shieldSimbol && i < text.Length - 1 && text[i + 1] == simbol) 
                {
                    curentText.Append(simbol);
                    i++;
                    continue;
                }
                if(text[i] == simbol && curentText.Length > 0)
                {
                    yield return curentText;
                    curentText.Clear();
                    continue;
                }
                curentText.Append(text[i]);
            }
            if (curentText.Length > 0)
                yield return curentText;
        }
    }
}
