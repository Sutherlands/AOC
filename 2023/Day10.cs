using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day10
  {
    public static void RunPart1()
    {
      var lines = new List<string> { "" };
      lines.AddRange(File.ReadAllLines("./PuzzleInputDay10.txt").Select(l => " " + l + " ").ToList());
      lines.Add(new string('.', lines[1].Length));
      lines[0] = new string('.', lines[1].Length);

      var grid = lines.Select(l => l.Select(c => GridLocation.Parse(c)).ToList()).ToList();
      var start = FindStart(lines);
      var startLocation = grid[start.Y][start.X];
      if (grid[start.Y - 1][start.X].Connections.HasFlag(Directions.South))
      {
        startLocation.Connections = startLocation.Connections | Directions.North;
      }
      if (grid[start.Y + 1][start.X].Connections.HasFlag(Directions.North))
      {
        startLocation.Connections = startLocation.Connections | Directions.South;
      }
      if (grid[start.Y][start.X - 1].Connections.HasFlag(Directions.East))
      {
        startLocation.Connections = startLocation.Connections | Directions.West;
      }
      if (grid[start.Y][start.X + 1].Connections.HasFlag(Directions.West))
      {
        startLocation.Connections = startLocation.Connections | Directions.East;
      }

      startLocation.Distance = 0;

      var queue = new Queue<Point>();
      queue.Enqueue(start);

      while (queue.TryDequeue(out var next))
      {
        UpdateGrid(next, queue, grid, Directions.North);
        UpdateGrid(next, queue, grid, Directions.South);
        UpdateGrid(next, queue, grid, Directions.West);
        UpdateGrid(next, queue, grid, Directions.East);
      }

      Console.WriteLine(grid.SelectMany(g => g).Where(gl => gl.Distance != int.MaxValue).Max(gl => gl.Distance));
    }

    private static void UpdateGrid(Point location, Queue<Point> queue, List<List<GridLocation>> grid, Directions direction)
    {
      var currentLocation = grid[location.Y][location.X];
      if (!currentLocation.Connections.HasFlag(direction))
      {
        return;
      }

      var newX = location.X + (direction == Directions.East ? 1 : direction == Directions.West ? -1 : 0);
      var newY = location.Y + (direction == Directions.South ? 1 : direction == Directions.North ? -1 : 0);
      var newLocation = grid[newY][newX];
      if (newLocation.Distance > currentLocation.Distance + 1)
      {
        newLocation.Distance = currentLocation.Distance + 1;
        queue.Enqueue(new Point(newX, newY));
      }
    }

    private static Point FindStart(List<string> lines)
    {
      for (int x = 0; x < lines[0].Length; ++x)
      {
        for (int y = 0; y < lines.Count; ++y)
        {
          if (lines[y][x] == 'S')
          {
            return new Point(x, y);
          }
        }
      }
      throw new Exception();
    }

    public static void RunPart2()
    {
      var lines = new List<string> { "" };
      lines.AddRange(File.ReadAllLines("./PuzzleInputDay10.txt").Select(l => " " + l + " ").ToList());
      lines.Add(new string('.', lines[1].Length));
      lines[0] = new string('.', lines[1].Length);

      var grid = lines.Select(l => l.Select(c => GridLocation.Parse(c)).ToList()).ToList();
      var start = FindStart(lines);
      var startLocation = grid[start.Y][start.X];
      if (grid[start.Y - 1][start.X].Connections.HasFlag(Directions.South))
      {
        startLocation.Connections = startLocation.Connections | Directions.North;
      }
      if (grid[start.Y + 1][start.X].Connections.HasFlag(Directions.North))
      {
        startLocation.Connections = startLocation.Connections | Directions.South;
      }
      if (grid[start.Y][start.X - 1].Connections.HasFlag(Directions.East))
      {
        startLocation.Connections = startLocation.Connections | Directions.West;
      }
      if (grid[start.Y][start.X + 1].Connections.HasFlag(Directions.West))
      {
        startLocation.Connections = startLocation.Connections | Directions.East;
      }

      startLocation.Distance = 0;

      var queue = new Queue<Point>();
      queue.Enqueue(start);

      while (queue.TryDequeue(out var next))
      {
        UpdateGrid(next, queue, grid, Directions.North);
        UpdateGrid(next, queue, grid, Directions.South);
        UpdateGrid(next, queue, grid, Directions.West);
        UpdateGrid(next, queue, grid, Directions.East);
      }

      for (int x = 0; x < grid[0].Count; ++x)
      {
        queue.Enqueue(new Point(x, 0));
        queue.Enqueue(new Point(x, grid[0].Count - 1));
      }

      for (int y = 0; y < grid.Count; ++y)
      {
        queue.Enqueue(new Point(0, y));
        queue.Enqueue(new Point(grid[0].Count - 1, y));
      }

      while (queue.TryDequeue(out var next))
      {
        UpdateGridOutside(next, queue, grid);
      }

      var count = 0;
      for (int y = 0; y < grid.Count; ++y)
      {
        for (int x = 0; x < grid[0].Count; ++x)
        {
          var gl = grid[y][x];
          if (gl.IsOutsideLoop == true)
          {
            Console.Write('O');
            continue;
          }
          if(gl.Distance != int.MaxValue)
          {
            Console.Write(lines[y][x]);
            continue;
          }

          var currentDirection = Directions.None;
          var crossedPipes = 0;
          for(var connectionX = x + 1; connectionX < grid[0].Count; ++connectionX)
          {
            var gridLocation = grid[y][connectionX];
            if (gridLocation.Distance == int.MaxValue)
            {
              continue;
            }
            switch(currentDirection)
            {
              case Directions.North:
                if(gridLocation.Connections.HasFlag(Directions.South))
                {
                  crossedPipes++;
                  currentDirection = Directions.None;
                }
                if(gridLocation.Connections.HasFlag(Directions.North))
                {
                  currentDirection = Directions.None;
                }
                break;
              case Directions.South:
                if (gridLocation.Connections.HasFlag(Directions.North))
                {
                  crossedPipes++;
                  currentDirection = Directions.None;
                }
                if (gridLocation.Connections.HasFlag(Directions.South))
                {
                  currentDirection = Directions.None;
                }
                break;
              case Directions.None:
                if(gridLocation.Connections.HasFlag(Directions.North))
                {
                  if(gridLocation.Connections.HasFlag(Directions.South))
                  {
                    crossedPipes++;
                  }
                  else
                  {
                    currentDirection = Directions.North;
                  }
                }
                else if(gridLocation.Connections.HasFlag(Directions.South))
                {
                  currentDirection = Directions.South;
                }
                break;
            }
          }

          if(crossedPipes % 2 == 1)
          {
            count++;
            Console.Write('I');
          }
          else
          {
            Console.Write('O');
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine(count);
    }

    [Flags]
    public enum Directions { None, North = 1, South = 2, East = 4, West = 8 }

    private static void UpdateGridOutside(Point next, Queue<Point> queue, List<List<GridLocation>> grid)
    {
      if (next.X < 0 || next.X >= grid[0].Count || next.Y < 0 || next.Y >= grid.Count)
      {
        return;
      }

      var gridLocation = grid[next.Y][next.X];
      if (gridLocation.IsOutsideLoop || gridLocation.Distance != int.MaxValue)
      {
        return;
      }

      gridLocation.IsOutsideLoop = true;
      for (int xMod = -1; xMod <= 1; ++xMod)
      {
        for (int yMod = -1; yMod <= 1; ++yMod)
        {
          if (xMod == 0 && yMod == 0)
          {
            continue;
          }
          queue.Enqueue(new Point(next.X + xMod, next.Y + yMod));
        }
      }
    }

    public class GridLocation
    {
      public Directions Connections = Directions.None;

      public int Distance = int.MaxValue;
      public bool IsOutsideLoop = false;

      public static GridLocation Parse(char c)
      {
        var gridLocation = new GridLocation();
        switch (c)
        {
          case '|':
            gridLocation.Connections = Directions.North | Directions.South;
            break;
          case '-':
            gridLocation.Connections = Directions.West | Directions.East;
            break;
          case 'L':
            gridLocation.Connections = Directions.North | Directions.East;
            break;
          case 'J':
            gridLocation.Connections = Directions.North | Directions.West;
            break;
          case 'F':
            gridLocation.Connections = Directions.East | Directions.South;
            break;
          case '7':
            gridLocation.Connections = Directions.West | Directions.South;
            break;
        }
        return gridLocation;
      }
    }
  }
}
