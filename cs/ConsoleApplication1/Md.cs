using System;
using ConsoleApplication1.Directions;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.Parsers;
using ConsoleApplication1.Renders;
using ConsoleApplication1.TextsConverter;

namespace ConsoleApplication1
{
    public class Md
    {
        private readonly char[] splitSymbols = { '_' };
        public string Render(string markDownText)
        {
            RaiseIfGivenStringIsNull(markDownText);
            var readerSymbols = new StringReader(markDownText);
            var parserText = new TextParser(readerSymbols, Array.AsReadOnly(splitSymbols), true);
            return Render(parserText);
        }

        private string Render(IParser textParse)
        {
            var render = new RenderMdToHtml(new DetectorTextDirection(),
                new ConverterMdSelectionsToHtmlData());
            while (textParse.AnyParts())
                render.AddNextPart(textParse.GetNextPart());
            RaiseIfTextParseDidNotSendEndText(render);
            return render.GetTranslatedText();
        }

        private void RaiseIfGivenStringIsNull(string markDownText)
        {
            if (markDownText == null)
                throw new ArgumentException("Md can not convert null instead of a text");
        }

        private void RaiseIfTextParseDidNotSendEndText(ITextRender render)
        {
            if (!render.IsTranslationFinished())
                throw new ArgumentException("Text parser did not send end at the end!");
        }
    }
}
