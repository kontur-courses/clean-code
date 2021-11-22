using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Samples
{
    public class PathFinder_Refactored
    {
        public static Point GetNextStepToTarget(Point source, Point target, IMaze maze)
        {
            var queue = new Queue<Point>();
            var used = new HashSet<Point>();
            queue.Enqueue(target);
            used.Add(target);
            while (queue.Any())
            {
                var p = queue.Dequeue();
                foreach (var neighbour in GetNeighbours(p, maze))
                {
                    if (used.Contains(neighbour)) continue;
                    if (neighbour == source)
                        return p;
                    queue.Enqueue(neighbour);
                    used.Add(neighbour);
                }
            }
            return source;
        }

        public static IMaze GenerateRandomWorld()
        {
            throw new NotImplementedException("TODO");
        }

        private static IEnumerable<Point> GetNeighbours(Point from, IMaze maze)
        {
            return new[] {new Size(1, 0), new Size(-1, 0), new Size(0, 1), new Size(0, -1)}
                .Select(shift => from + shift)
                .Where(maze.InsideMaze)
                .Where(maze.IsFree);
        }
    }
}