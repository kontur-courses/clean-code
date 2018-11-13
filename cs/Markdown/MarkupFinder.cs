using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkupFinder
    {
        private readonly List<Markup> markups;
        private readonly Dictionary<Markup, List<int>> openingPositionsForMarkups = new Dictionary<Markup, List<int>>();
        private readonly Dictionary<Markup, List<int>> closingPositionsForMarkups = new Dictionary<Markup, List<int>>();

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

                var shift = 0;

                if (openingMarkup != null)
                {
                    if (!openingPositionsForMarkups.ContainsKey(openingMarkup))
                        openingPositionsForMarkups[openingMarkup] = new List<int>();
                    openingPositionsForMarkups[openingMarkup].Add(index);
                    if (shift < openingMarkup.Template.Length)
                        shift = openingMarkup.Template.Length;
                }
                if (closingMarkup != null)
                {
                    if (!closingPositionsForMarkups.ContainsKey(closingMarkup))
                        closingPositionsForMarkups[closingMarkup] = new List<int>();
                    closingPositionsForMarkups[closingMarkup].Add(index);
                    if (shift < closingMarkup.Template.Length)
                        shift = closingMarkup.Template.Length;
                }

                if (shift != 0)
                    index += shift - 1;
            }
        }

        private Dictionary<Markup, List<MarkupPosition>> GetMarkupBoarders()
        {
            var dict = new Dictionary<Markup, List<MarkupPosition>>();

            foreach (var openingPositionsForMarkup in openingPositionsForMarkups)
            {
                var markup = openingPositionsForMarkup.Key;
                if (!closingPositionsForMarkups.ContainsKey(markup)) continue;
                
                dict.Add(markup, GetPositionsForMarkup(markup));
            }

            return dict;
        }

        private List<MarkupPosition> GetPositionsForMarkup(Markup markup)
        {
            var positionsForMarkups = new List<MarkupPosition>();

            var openingPositions = new SortedSet<int>(openingPositionsForMarkups[markup]);
            var closingPositions = new SortedSet<int>(closingPositionsForMarkups[markup]);

            var usedPositions = new HashSet<int>();

            foreach (var openingPosition in openingPositions.Reverse())
            {
                if (usedPositions.Contains(openingPosition))
                    continue;
                var closingPosition = closingPositions
                    .FirstOrDefault(
                        position => 
                            position > openingPosition &&
                            !usedPositions.Contains(position));
                if (closingPosition == 0)
                    continue;
                
                positionsForMarkups.Add(new MarkupPosition(openingPosition, closingPosition));
                usedPositions.Add(openingPosition);
                usedPositions.Add(closingPosition);
            }

            return positionsForMarkups;
        }

        public Dictionary<Markup, List<MarkupPosition>> GetMarkupsWithPositions(string paragraph)
        {
            FindOpeningAndClosingTemplates(paragraph);

            return GetMarkupBoarders();
        }
    }
}
