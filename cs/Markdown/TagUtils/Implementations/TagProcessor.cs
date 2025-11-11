using System.Text;
using Markdown.Dto;
using Markdown.TagUtils.Abstractions;
using Markdown.TokensUtils;

namespace Markdown.TagUtils.Implementations;

public class TagProcessor(ITagContext context, TagStrategyFactory factory) : ITagProcessor
{
    public string Process(IEnumerable<Token> tokens)
    {
        using (context)
        {
            foreach (var token in tokens)
            {
                if (context.SkipNextAsMarkup)
                {
                    context.Append(token.Value);
                    context.SkipNextAsMarkup = false;
                    continue;
                }

                var strategy = factory.Get(token.Type);
                strategy?.Process(token);
            }

            return context.Content.ToString();
        }
    }
}