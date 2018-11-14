using System;
using ConsoleApplication1.Interfaces;

namespace ConsoleApplication1
{
    public class RenderMdToHtml : ITextRender
    {
        private bool isFinished;
        public void AddNextPart(TextPart text)
        {
            throw new NotImplementedException();
        }

        public string GetTranslatedText()
        {
            throw new NotImplementedException();
        }

        public bool IsTranslationFinished()
            => isFinished;

        public RenderMdToHtml()
        {

            throw new NotImplementedException();
        }
    }
}
