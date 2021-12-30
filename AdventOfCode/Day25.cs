using DotNetty.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode
{
  public static class Day25
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay25.txt").ToList();
      var maxX = lines[0].Length;
      var maxY = lines.Count();

      var grid = new char[maxX, maxY];
      for(var yIndex = 0; yIndex < maxY; ++yIndex)
      {
        for(var xIndex = 0; xIndex < maxX; ++xIndex)
        {
          var c = lines[yIndex][xIndex];
          if (c != '.')
          {
            grid[xIndex, yIndex] = c;
          }
        }
      }

      bool isMoving = true;
      var newGrid = new char[maxX, maxY];
      var steps = 0;

      while(isMoving)
      {
        steps++;
        isMoving = false;
        newGrid = new char[maxX, maxY];

        for(var y = 0; y < maxY; ++y)
        {
          for(var x = 0; x < maxX; ++x)
          {
            if(grid[x,y] == '>')
            {
              var newX = x + 1;
              if(newX == maxX)
              {
                newX = 0;
              }

              if(grid[newX, y] == default)
              {
                newGrid[newX, y] = '>';

                isMoving = true;
              }
              else
              {
                newGrid[x, y] = '>';
              }
            }
            else if(grid[x,y] == 'v')
            {
              newGrid[x, y] = 'v';
            }
          }
        }


        grid = newGrid;
        newGrid = new char[maxX, maxY];


        for (var y = 0; y < maxY; ++y)
        {
          for (var x = 0; x < maxX; ++x)
          {
            if (grid[x, y] == 'v')
            {
              var newY = y + 1;
              if (newY == maxY)
              {
                newY = 0;
              }

              if (grid[x, newY] == default)
              {
                newGrid[x, newY] = 'v';

                isMoving = true;
              }
              else
              {
                newGrid[x, y] = 'v';
              }
            }
            else if (grid[x, y] == '>')
            {
              newGrid[x, y] = '>';
            }
          }
        }

        grid = newGrid;
      }

      Console.WriteLine(steps);

    }
  }
}
