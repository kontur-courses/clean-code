using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Samples
{
    public static class PathFinder
    {
        public static Point GetNextStepToTarget(IMaze maze, Point source, Point target)
        {
            var queue = new Queue<Point>();
            queue.Enqueue(target);
            var used = new HashSet<Point> {target};
            
            while (queue.Any())
            {
                var p = queue.Dequeue();
                foreach (var neighbour in GetNeighbours(maze, p))
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

        private static IEnumerable<Point> GetNeighbours(IMaze maze, Point from)
        {
            return new[] { new Size(1, 0), new Size(-1, 0), new Size(0, 1), new Size(0, -1) }
                .Select(shift => from + shift)
                .Where(maze.InsideMaze)
                .Where(maze.IsFree);
        }
    }

    public interface IMaze
    {
        bool InsideMaze(Point location);
        bool IsFree(Point location);
    }
}