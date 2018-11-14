using System;

namespace ConsoleApplication1.Interfaces
{
    public interface ITextRender
    {
        void AddNextPart(TextPart text);
        bool IsTranslationFinished();
        String GetTranslatedText();
    }
}
