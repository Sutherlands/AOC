using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2024
{
    public static class Day16
    {
        private enum Direction
        {
            North, East, South, West
        }

        private class Status : IEquatable<Status>
        {
            public (int x, int y) Location { get; set; }
            public Direction Direction { get; set; }
            public int Cost { get; set; }

            public bool Equals(Status other)
            {
                return other.Location.Equals(Location) && other.Cost == Cost && other.Direction == Direction;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public static void RunPart1()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay16.txt").ToList();


            (int x, int y) startPosition = default;

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Count; y++)
                {
                    if (lines[y][x] == 'S')
                    {
                        startPosition = (x, y);
                    }
                }
            }

            Console.WriteLine(GetShortestPathValue(lines, startPosition).value);
        }

        private static (int value, List<Status> processedNodes) GetShortestPathValue(List<string> lines, (int x, int y) startPosition)
        {
            var processedNodes = new List<Status>();
            var minimumCost = lines.Select(l => l.Select(c => int.MaxValue).ToList()).ToList();
            var queue = new PriorityQueue<Status, int>();
            queue.Enqueue(new Status { Location = startPosition, Direction = Direction.East }, 0);

            while (true)
            {
                var node = queue.Dequeue();
                if (lines[node.Location.y][node.Location.x] == 'E')
                {
                    processedNodes.Add(node);
                    return (node.Cost, processedNodes);
                }

                if (minimumCost[node.Location.y][node.Location.x] <= node.Cost - 1000)
                {
                    continue;
                }

                if (lines[node.Location.y][node.Location.x] == '#')
                {
                    continue;
                }

                processedNodes.Add(node);
                minimumCost[node.Location.y][node.Location.x] = Math.Min(minimumCost[node.Location.y][node.Location.x], node.Cost);

                var forwardNode = new Status { Location = ModifyLocation(node.Location, node.Direction), Direction = node.Direction, Cost = node.Cost + 1 };
                var leftNode = new Status { Location = ModifyLocation(node.Location, TurnLeft(node.Direction)), Direction = TurnLeft(node.Direction), Cost = node.Cost + 1001 };
                var rightNode = new Status { Location = ModifyLocation(node.Location, TurnRight(node.Direction)), Direction = TurnRight(node.Direction), Cost = node.Cost + 1001 };

                queue.Enqueue(forwardNode, forwardNode.Cost);
                queue.Enqueue(leftNode, leftNode.Cost);
                queue.Enqueue(rightNode, rightNode.Cost);
            }
        }

        private static void Print(List<string> lines, List<List<int>> minimumCost)
        {
            for (int y = 0; y < lines[0].Length; ++y)
            {
                for (int x = 0; x < lines.Count; ++x)
                {
                    Console.Write(lines[y][x] == '#' ? '#' : minimumCost[y][x] == int.MaxValue ? '.' : 'O');
                }

                Console.WriteLine();
            }
        }

        private static Direction TurnLeft(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Direction.West;
                case Direction.South: return Direction.East;
                case Direction.East: return Direction.North;
                case Direction.West: return Direction.South;
                default: throw new Exception();
            }
        }

        private static Direction TurnRight(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Direction.East;
                case Direction.South: return Direction.West;
                case Direction.East: return Direction.South;
                case Direction.West: return Direction.North;
                default: throw new Exception();
            }
        }

        private static (int x, int y) ModifyLocation((int x, int y) location, Direction direction)
        {
            var locationInternal = (location.x, location.y);
            switch (direction)
            {
                case Direction.North:
                    locationInternal.y--;
                    break;
                case Direction.South:
                    locationInternal.y++;

                    break;
                case Direction.East:
                    locationInternal.x++;

                    break;
                case Direction.West:
                    locationInternal.x--;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return locationInternal;
        }

        public static void RunPart2()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay16.txt").ToList();
            var pathSpots = lines.Select(l => l.Select(c => '.').ToList()).ToList();

            (int x, int y) startPosition = default;
            (int x, int y) endPosition = default;

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Count; y++)
                {
                    if (lines[y][x] == 'S')
                    {
                        startPosition = (x, y);
                    }

                    if (lines[y][x] == 'E')
                    {
                        endPosition = (x, y);
                    }
                }
            }

            var shortestPath = GetShortestPathValue(lines, startPosition);

            var nodesInAShortestPath = new HashSet<(int x, int y)>();

            var checkedNodes = new HashSet<Status>();
            var nodeQueue = new Queue<Status>();
            nodeQueue.Enqueue(new Status { Cost = shortestPath.value, Location = endPosition, Direction = Direction.North });
            nodeQueue.Enqueue(new Status { Cost = shortestPath.value, Location = endPosition, Direction = Direction.East });
            nodesInAShortestPath.Add(endPosition);

            while(nodeQueue.TryDequeue(out var node))
            {
                if(checkedNodes.Contains(node))
                {
                    continue;
                }

                checkedNodes.Add(node);

                foreach(var processedNode in shortestPath.processedNodes)
                {
                    if(CheckCost(processedNode, node))
                    {
                        nodesInAShortestPath.Add(processedNode.Location);
                        nodeQueue.Enqueue(processedNode);
                    }
                }
            }

            for (int y = 0; y < lines[0].Length; ++y)
            {
                for (int x = 0; x < lines.Count; ++x)
                {
                    Console.Write(lines[y][x] == '#' ? '#' : nodesInAShortestPath.Contains((x, y)) ? 'O' : '.');
                }

                Console.WriteLine();
            }
            //###############
            //#.......#....O#
            //#.#.###.#.###O#
            //#.....#.#...#O#
            //#.###.#####.#O#
            //#.#.#.......#O#
            //#.#.#####.###O#
            //#....OOOOOOO#O#
            //###.#O#####O#O#
            //#...#O....#O#O#
            //#.#.#O###.#O#O#
            //#OOOOO#...#O#O#
            //#O###.#.#.#O#O#
            //#O..#.....#OOO#
            //###############


            Console.WriteLine(nodesInAShortestPath.Count);
        }

        private static bool CheckCost(Status firstNode, Status secondNode)
        {
            var forwardNode = new Status { Location = ModifyLocation(firstNode.Location, firstNode.Direction), Direction = firstNode.Direction, Cost = firstNode.Cost + 1 };
            var leftNode = new Status { Location = ModifyLocation(firstNode.Location, TurnLeft(firstNode.Direction)), Direction = TurnLeft(firstNode.Direction), Cost = firstNode.Cost + 1001 };
            var rightNode = new Status { Location = ModifyLocation(firstNode.Location, TurnRight(firstNode.Direction)), Direction = TurnRight(firstNode.Direction), Cost = firstNode.Cost + 1001 };

            var secondLeft = new Status { Location = secondNode.Location, Direction = TurnLeft(secondNode.Direction), Cost = secondNode.Cost + 1000 };
            var secondRight = new Status { Location = secondNode.Location, Direction = TurnRight(secondNode.Direction), Cost = secondNode.Cost + 1000 };
            return new [] { forwardNode, leftNode, rightNode }.Intersect(new[] { secondNode, secondLeft, secondRight }).Any();
        }
    }
}
