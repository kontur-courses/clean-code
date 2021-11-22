using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Samples
{
    public class PathFinder
    {
        public static IMaze maze;
        private static readonly Queue<Point> queue = new Queue<Point>();
        private static readonly ISet<Point> used = new HashSet<Point>();

        public static void GenerateRandomMaze() { /* maze = ... */ }

        public static Point GetNextStepToTarget(Point source, Point target)
        {
            queue.Clear();
            used.Clear();
            queue.Enqueue(target);
            used.Add(target);
            while (queue.Any())
            {
                var p = queue.Dequeue();
                foreach (var neighbour in GetNeighbours(p))
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

        private static IEnumerable<Point> GetNeighbours(Point from)
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