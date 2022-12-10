using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MattsDay9
{
  public static void Part1()
  {
    var inputStream = new StreamReader("PuzzleInputDay9.txt");
    var rope = new Rope();
    var line = inputStream.ReadLine();
    while (!string.IsNullOrWhiteSpace(line))
    {
      var direction = line[0];
      var distance = line[2] - 48;
      rope.Move(direction, distance);
      line = inputStream.ReadLine();
    }
    var uniqueTailLocations = rope.TailLocations.GroupBy(p => new { p.x, p.y }).ToList();
    Console.WriteLine(uniqueTailLocations.Count);
  }

  public static void Part2()
  {
    var grid = File.ReadAllLines("inputs/09.txt");
  }

  public class Rope
  {
    public Rope()
    {
      Head = new Point();
      Tail = new Point();
      LastHead = new Point();
      TailLocations = new List<Point>();
    }
    public Point Head { get; set; }
    public Point Tail { get; set; }

    Point LastHead { get; set; }
    public void Move(char direction, int distance)
    {
      int xDir = 0;
      int yDir = 0;
      switch (direction)
      {
        case 'L': xDir = -1; break;
        case 'R': xDir = 1; break;
        case 'U': yDir = 1; break;
        case 'D': yDir = -1; break;
      }
      for (var i = 0; i < distance; i++)
      {
        LastHead = new Point(Head);
        Head.x += xDir;
        Head.y += yDir;
        ResolveTail();
      }
    }
    void ResolveTail()
    {
      if (!Head.isAdjacent(Tail))
      {
        var xDist = Head.x - Tail.x;
        var yDist = Head.y - Tail.y;
        if (xDist != 0 && yDist != 0)
        {
          Tail.x = LastHead.x;
          Tail.y = LastHead.y;
        }
        else if (xDist != 0) Tail.x += Math.Sign(xDist);
        else if (yDist != 0) Tail.y += Math.Sign(yDist);
      }
      TailLocations.Add(new Point(Tail));
    }

    public List<Point> TailLocations { get; set; }
  }

  public class Point
  {
    public Point() { }
    public Point(Point p)
    {
      x = p.x;
      y = p.y;
    }
    public bool isAdjacent(Point p)
    {
      return ((p.x == x && p.y == y) ||
              (p.x == x - 1 && p.y == y) ||
              (p.x == x + 1 && p.y == y) ||
              (p.x == x && p.y == y + 1) ||
              (p.x == x && p.y == y - 1) ||
              (p.x == x + 1 && p.y == y + 1) ||
              (p.x == x - 1 && p.y == y - 1) ||
              (p.x == x + 1 && p.y == y - 1) ||
              (p.x == x - 1 && p.y == y + 1));
    }
    public int x { get; set; }
    public int y { get; set; }
  }
}