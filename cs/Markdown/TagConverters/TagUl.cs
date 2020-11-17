using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class TagUl : TagConverterBase
    {
        public override bool IsSingleTag => false;

        public override HashSet<string> TagInside => new HashSet<string>();

        public override string Html => TagHtml.ul;

        public override string Md => TagMd.list;

        public override bool CanClose(StringBuilder text, int pos) => CanCloseBase(text, pos);

        public override bool CanOpen(StringBuilder text, int pos) => CanOpenBase(text, pos);

        public override bool IsTag(string text, int pos) => true;

        public StringBuilder Convert(StringBuilder text)
        {
            var result = new StringBuilder();
            result.Append(StringMd);
            foreach(var tagText in Split(text, ';'))
            {
                result.Append(new TagLi().FormTags(Markdown.Md.Render(tagText)));
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
