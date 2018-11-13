using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkupFinder
    {
        private readonly List<Markup> markups;
        private readonly Dictionary<int, Markup> openingPositions = new Dictionary<int, Markup>();
        private readonly Dictionary<int, Markup> closingPositions = new Dictionary<int, Markup>();

        public MarkupFinder(List<Markup> markups)
        {
            this.markups = markups;
        }

        private void FindOpeningAndClosingTemplates(string paragraph)
        {
            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingMarkup = markups.GetOpeningMarkup(paragraph, index);
                var closingMarkup = markups.GetClosingMarkup(paragraph, index);

                if (openingMarkup != null)
                    openingPositions[index] = openingMarkup;
                if (closingMarkup != null)
                    closingPositions[index] = closingMarkup;
            }
        }

        private Dictionary<Markup, List<MarkupPosition>> GetMarkupBoarders()
        {
            var dict = new Dictionary<Markup, List<MarkupPosition>>();

            var stackOfOpening = new Stack<int>(openingPositions.Keys);
            var queueOfClosing = new Queue<int>(closingPositions.Keys);

            while (stackOfOpening.Count > 0 && queueOfClosing.Count > 0)
            {
                var openingPosition = stackOfOpening.Pop();
                var closingPosition = queueOfClosing.Dequeue();

                var markup = openingPositions[openingPosition];

                if (!dict.ContainsKey(markup))
                    dict.Add(markup, new List<MarkupPosition>());
                dict[markup].Add(new MarkupPosition(openingPosition, closingPosition));
            }
            return dict;
        }

        public Dictionary<Markup, List<MarkupPosition>> GetMarkupsWithPositions(string paragraph)
        {
            FindOpeningAndClosingTemplates(paragraph);

            return GetMarkupBoarders();
        }
    }
}
