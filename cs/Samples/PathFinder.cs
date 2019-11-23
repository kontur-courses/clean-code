using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Samples
{
    public class PathFinder
    {
        public static IMaze Maze;
        private static readonly Queue<Point> Queue = new Queue<Point>();
        private static readonly ISet<Point> Used = new HashSet<Point>();

        public static void GenerateRandomMaze() { /* maze = ... */ }

        public static Point GetNextStepToTarget(Point source, Point target)
        {
            Queue.Clear();
            Used.Clear();
            Queue.Enqueue(target);
            Used.Add(target);
            while (Queue.Any())
            {
                var p = Queue.Dequeue();
                foreach (var neighbour in GetNeighbours(p))
                {
                    if (Used.Contains(neighbour)) continue;
                    if (neighbour == source)
                        return p;
                    Queue.Enqueue(neighbour);
                    Used.Add(neighbour);
                }
            }
            return source;
        }

        private static IEnumerable<Point> GetNeighbours(Point from)
        {
            return new[] { new Size(1, 0), new Size(-1, 0), new Size(0, 1), new Size(0, -1) }
                .Select(shift => from + shift)
                .Where(Maze.InsideMaze)
                .Where(Maze.IsFree);
        }
    }

    public interface IMaze
    {
        bool InsideMaze(Point location);
        bool IsFree(Point location);
    }
}