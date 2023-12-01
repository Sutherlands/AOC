using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  public static class Day14
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();

      var locations = new char[1000][];
      for (int x = 0; x < 1000; ++x)
      {
        locations[x] = new char[200];
      }

      foreach (var line in lines)
      {
        var split = line.Split(" -> ");

        var pointSplit = split[0].Split(',');
        var previousX = int.Parse(pointSplit[0]);
        var previousY = int.Parse(pointSplit[1]);
        locations[previousX][previousY] = 'r';

        for (int x = 1; x < split.Length; ++x)
        {
          var newSplit = split[x].Split(',');
          var newX = int.Parse(newSplit[0]);
          var newY = int.Parse(newSplit[1]);

          var xChange = Math.Sign(newX - previousX);
          var yChange = Math.Sign(newY - previousY);

          while (previousX != newX || previousY != newY)
          {
            previousX += xChange;
            previousY += yChange;
            locations[previousX][previousY] = 'r';
          }
        }
      }

      while (true)
      {
        var currentSandX = 500;
        var currentSandY = 0;
        var stopped = false;

        while (true)
        {
          if (currentSandY + 1 >= 200)
          {
            break;
          }
          if (locations[currentSandX][currentSandY + 1] == 0)
          {
            currentSandY++;
            continue;
          }
          if (locations[currentSandX - 1][currentSandY + 1] == 0)
          {
            currentSandX--;
            currentSandY++;
            continue;
          }
          if (locations[currentSandX + 1][currentSandY + 1] == 0)
          {
            currentSandX++;
            currentSandY++;
            continue;
          }
          locations[currentSandX][currentSandY] = 's';
          stopped = true;
          break;
        }

        if (!stopped)
        {
          break;
        }
      }


      Console.WriteLine(locations.SelectMany(l => l).Where(l => l == 's').Count());
      Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}");
    }

    public static void Print(char[][] locations)
    {
      int minY = 1000, maxY = 0, minX = 1000, maxX = 0;
      for (var x = 0; x < locations.Length; ++x)
      {
        for (var y = 0; y < locations[0].Length; ++y)
        {
          if (locations[x][y] != 0)
          {
            minY = Math.Min(minY, y);
            maxY = Math.Max(maxY, y);
            minX = Math.Min(minX, x);
            maxX = Math.Max(maxX, x);
          }
        }
      }

      for (int y = minY - 1; y < maxY + 1; ++y)
      {

        for (int x = minX - 1; x < maxX + 1; ++x)
        {
          Console.Write(locations[x][y] == 0 ? ' ' : locations[x][y]);
        }
        Console.WriteLine();
      }
    }


    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();

      var locations = new char[1000][];
      for (int x = 0; x < 1000; ++x)
      {
        locations[x] = new char[200];
      }

      var highestY = 0;

      foreach (var line in lines)
      {
        var split = line.Split(" -> ");

        var pointSplit = split[0].Split(',');
        var previousX = int.Parse(pointSplit[0]);
        var previousY = int.Parse(pointSplit[1]);
        locations[previousX][previousY] = 'r';
        highestY = Math.Max(highestY, previousY);

        for (int x = 1; x < split.Length; ++x)
        {
          var newSplit = split[x].Split(',');
          var newX = int.Parse(newSplit[0]);
          var newY = int.Parse(newSplit[1]);
          highestY = Math.Max(highestY, newY);

          var xChange = Math.Sign(newX - previousX);
          var yChange = Math.Sign(newY - previousY);

          while (previousX != newX || previousY != newY)
          {
            previousX += xChange;
            previousY += yChange;
            locations[previousX][previousY] = 'r';
          }
        }
      }

      var floorLocation = highestY + 2;
      for(int x = 0; x < 1000; ++x)
      {
        locations[x][floorLocation] = 'r';
      }

      while (true)
      {
        var currentSandX = 500;
        var currentSandY = 0;
        var moved = true;

        while (true)
        {
          if (currentSandY + 1 >= 200)
          {
            break;
          }
          if (locations[currentSandX][currentSandY + 1] == 0)
          {
            currentSandY++;
            continue;
          }
          if (locations[currentSandX - 1][currentSandY + 1] == 0)
          {
            currentSandX--;
            currentSandY++;
            continue;
          }
          if (locations[currentSandX + 1][currentSandY + 1] == 0)
          {
            currentSandX++;
            currentSandY++;
            continue;
          }
          locations[currentSandX][currentSandY] = 's';
          if(currentSandX == 500 && currentSandY == 0)
          {
            moved = false;
          }
          break;
        }

        if (!moved)
        {
          break;
        }
      }

      Print(locations);

      Console.WriteLine(locations.SelectMany(l => l).Where(l => l == 's').Count());
      Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}");
    }
  }
}
