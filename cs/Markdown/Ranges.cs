using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Ranges
    {
        private List<Tuple<int, int>> ranges;

        public Ranges()
        {
            ranges = new List<Tuple<int, int>>();
        }

        public void AddRange(int startIndex, int endIndex)
        {
            ranges.Add(Tuple.Create(startIndex, endIndex));
        }

        public bool IsIndexInRange(int index)
        {
            foreach (var range in ranges)
            {
                if (index >= range.Item1 && index <= range.Item2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
