using System;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.Parsers;

namespace ConsoleApplication1
{
    public class Md
    {
        private readonly char[] splitSymbols = new char[] {'\\'};
         public string Render(string markDownText)
        {
            throw new NotImplementedException();
            var readerSymbols = new StringReader(markDownText);
            var parserText = new TextParser(readerSymbols, Array.AsReadOnly(splitSymbols), true);
            return Render(parserText);
        }

        private string Render(IParser textParse)
        {
            throw new NotImplementedException();
            var render = new RenderMdToHtml();
            RaiseIfTextParseDidNotSendEndText(render);    
            return render.GetTranslatedText();
        }

        private void RaiseIfTextParseDidNotSendEndText(ITextRender render)
        {
           throw new ArgumentException("Text parser did not send end at the end!");
        }
    }
}
