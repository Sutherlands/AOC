using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  public static class Day12
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();
      var mapHeight = lines.Count;
      var mapLength = lines[0].Length;

      var height = new int[mapLength][];
      var distance = new int[mapLength][];
      int startX = 0, startY = 0, endX = 0, endY = 0;
      for(int x = 0; x < mapLength; ++x)
      {
        height[x] = new int[mapHeight];
        distance[x] = new int[mapHeight];
      }

      for(int y = 0; y < mapHeight; ++y)
      {
        for(int x = 0; x < mapLength; ++x)
        {
          var c = lines[y][x];
          if(c == 'S')
          {
            startX = x;
            startY = y;
            c = 'a';
          }
          if(c == 'E')
          {
            endX = x;
            endY = y;
            c = 'z';
          }

          height[x][y] = c - 'a';
          distance[x][y] = 999999;
        }
      }

      Console.WriteLine($"Map size {mapLength}x{mapHeight}");
      distance[endX][endY] = 0;
      var queue = new List<QueueData>();
      queue.Add(new QueueData { Height = height[endX][endY], X = endX, Y = endY });
      var totalProcessed = 0;
      while(queue.Count > 0)
      {
        ++totalProcessed;
        var current = queue.OrderBy(i => distance[i.X][i.Y]).First();
        queue.Remove(current);
        if(current.X > 0)
        {
          var newX = current.X - 1;
          var newY = current.Y;
          if(height[newX][newY] + 1 >= height[current.X][current.Y])
          {
            if(distance[current.X][current.Y] + 1 < distance[newX][newY])
            {
              distance[newX][newY] = distance[current.X][current.Y] + 1;
              queue.Add(new QueueData { Height = distance[newX][newY], X = newX, Y = newY });
            }
          }
        }
        if(current.Y > 0)
        {
          var newX = current.X;
          var newY = current.Y-1;
          if (height[newX][newY] + 1 >= height[current.X][current.Y])
          {
            if (distance[current.X][current.Y] + 1 < distance[newX][newY])
            {
              distance[newX][newY] = distance[current.X][current.Y] + 1;
              queue.Add(new QueueData { Height = distance[newX][newY], X = newX, Y = newY });
            }
          }
        }
        if(current.X < mapLength - 1)
        {
          var newX = current.X + 1;
          var newY = current.Y;
          if (height[newX][newY] + 1 >= height[current.X][current.Y])
          {
            if (distance[current.X][current.Y] + 1 < distance[newX][newY])
            {
              distance[newX][newY] = distance[current.X][current.Y] + 1;
              queue.Add(new QueueData { Height = distance[newX][newY], X = newX, Y = newY });
            }
          }
        }
        if (current.Y < mapHeight - 1)
        {
          var newX = current.X;
          var newY = current.Y + 1;
          if (height[newX][newY] + 1 >= height[current.X][current.Y])
          {
            if (distance[current.X][current.Y] + 1 < distance[newX][newY])
            {
              distance[newX][newY] = distance[current.X][current.Y] + 1;
              queue.Add(new QueueData { Height = distance[newX][newY], X = newX, Y = newY });
            }
          }
        }
      }

      Console.WriteLine(distance[startX][startY]);

      var shortestDistance = 9999999;
      for(var x = 0; x < mapLength; ++x)
      {
        for(var y = 0; y < mapHeight; ++ y)
        {
          if(height[x][y] == 0)
          {
            shortestDistance = Math.Min(shortestDistance, distance[x][y]);
          }
        }
      }
      Console.WriteLine(shortestDistance);
      Console.WriteLine($"Processed {totalProcessed} in {sw.ElapsedMilliseconds}");
    }

    public static void RunPart2()
    {
    }

    public class QueueData
    {
      public int Height { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
    }


  }
}
