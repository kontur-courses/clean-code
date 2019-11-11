using Markdown.MdTokens;

namespace Markdown.MdProcessing
{
    public interface IMdProcessor
    {
        string GetProcessedResult(MdToken token);
    }
}