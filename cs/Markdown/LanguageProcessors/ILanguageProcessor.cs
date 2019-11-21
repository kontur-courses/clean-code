using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Validators;
using Markdown.Wrappers;

namespace Markdown.LanguageProcessors
{
    public interface ILanguageProcessor
    {
        List<ITag> Tags { get; }
        Dictionary<string, bool> IsOpen { get; }
        Wrapper Wrapper { get; }
        Validator Validator { get; }
        string Render(string input);
    }
}