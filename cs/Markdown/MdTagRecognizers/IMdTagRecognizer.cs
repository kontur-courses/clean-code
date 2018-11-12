using System;

namespace Markdown.MdTagRecognizers
{
    public interface IMdTagRecognizer : IDisposable
    {
        bool TryRecognize(string str, int position, out MdType type);
    }
}