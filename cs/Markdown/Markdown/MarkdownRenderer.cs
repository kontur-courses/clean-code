using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models;
using Markdown.Paragraphs;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer
    {
        private readonly List<IToken> tokensToRender = MarkdownRenderConfigFactory.GetTokens().ToList();

        private readonly Dictionary<ParagraphType, IParagraphGroup> paragraphGroups =
            MarkdownRenderConfigFactory.GetParagraphGroups();

        private readonly TokenEscaper escaper;
        private readonly TokenReader reader;

        public MarkdownRenderer()
        {
            escaper = new TokenEscaper(tokensToRender);
            reader = new TokenReader(tokensToRender);
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var paragraphs = text.Split('\n', '\r')
                .Where(line => line != "")
                .Select(ParseParagraph);

            var groups = GroupParagraphs(paragraphs);
            var linesToRender = GetLinesToRender(groups);

            return string.Join('\n', linesToRender);
        }

        private Paragraph ParseParagraph(string text)
        {
            var matches = reader.FindAll(text).ToList();
            var matchesMap = new MatchesMap(matches.ToList());

            var paragraphType = GetParagraphType(matches);
            var content = ConvertText(text, matchesMap);
            return new Paragraph(paragraphType, content);
        }

        private static ParagraphType GetParagraphType(IReadOnlyCollection<TokenMatch> matches)
        {
            var paragraphType = ParagraphType.Text;

            if (matches.Any(match => match.Token.TagType == TagType.Header))
                paragraphType = ParagraphType.Header;

            if (matches.Any(match => match.Token.TagType == TagType.UnorderedList))
                paragraphType = ParagraphType.UnorderedList;

            return paragraphType;
        }

        private string ConvertText(string text, MatchesMap matchesMap)
        {
            var builder = new StringBuilder();
            var context = new Context(text);
            for (var i = 0; i <= text.Length; i++)
            {
                context.Index = i;
                var nextPart = GetNextPart(matchesMap, context);
                builder.Append(nextPart.Text);
                i = nextPart.Index;
            }

            return builder.ToString();
        }

        private Context GetNextPart(MatchesMap matchesMap, Context context)
        {
            if (matchesMap.TryGetTagReplacerAtPosition(context.Index, out var replacer))
                return new Context(replacer.Tag, context.Index + replacer.TrimLength - 1);

            if (context.Index < context.Text.Length)
                return escaper.IsEscapeSymbol(context)
                    ? new Context(context.Skip(1).CurrentSymbol.ToString(), context.Index + 1)
                    : new Context(context.CurrentSymbol.ToString(), context.Index);

            return new Context("", context.Index);
        }

        private static List<Group<ParagraphType, Paragraph>> GroupParagraphs(IEnumerable<Paragraph> paragraphs)
        {
            var groups = new List<Group<ParagraphType, Paragraph>>();
            foreach (var paragraph in paragraphs)
            {
                if (groups.Count == 0 || groups.Last().Key != paragraph.Type)
                    groups.Add(new Group<ParagraphType, Paragraph>(paragraph.Type));

                groups.Last().Values.Add(paragraph);
            }

            return groups;
        }

        private IEnumerable<string> GetLinesToRender(List<Group<ParagraphType, Paragraph>> groups)
        {
            var linesToRender = new List<string>();
            foreach (var group in groups)
            {
                if (paragraphGroups.TryGetValue(group.Key, out var paragraphGroup))
                    linesToRender.Add(paragraphGroup.OpenTag);

                linesToRender.AddRange(group.Values.Select(p => p.Content));

                if (paragraphGroup != null)
                    linesToRender.Add(paragraphGroup.CloseTag);
            }

            return linesToRender;
        }
    }
}