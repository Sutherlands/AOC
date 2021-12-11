using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day10
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();
      var sum = 0;
      foreach(var line in lines)
      {
        sum += GetCorruptedStackScore(line);
      }
      Console.WriteLine(sum);
    }

    private static int GetCorruptedStackScore(string line)
    {
      var stack = new Stack<char>();

      foreach(var c in line)
      {
        switch(c)
        {
          case '{':
          case '(':
          case '[':
          case '<':
            stack.Push(c);
            break;
          case '}':
            if (stack.Pop() != '{')
              return 1197;
            break;
          case ')':
            if (stack.Pop() != '(')
              return 3;
            break;
          case ']':
            if (stack.Pop() != '[')
              return 57;
            break;
          case '>':
            if (stack.Pop() != '<')
              return 25137;
            break;
        }
      }

      return 0;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();
      var scores = lines.Select(l => GetIncompleteStackScore(l)).Where(s => s != 0).ToList();
      scores.Sort();
      var targetIndex = (scores.Count - 1) / 2;
      Console.WriteLine(scores[targetIndex]);
    }

    private static long GetIncompleteStackScore(string line)
    {
      var stack = new Stack<char>();

      foreach (var c in line)
      {
        switch (c)
        {
          case '{':
          case '(':
          case '[':
          case '<':
            stack.Push(c);
            break;
          case '}':
            if (stack.Pop() != '{')
              return 0;
            break;
          case ')':
            if (stack.Pop() != '(')
              return 0;
            break;
          case ']':
            if (stack.Pop() != '[')
              return 0;
            break;
          case '>':
            if (stack.Pop() != '<')
              return 0;
            break;
        }
      }

      var score = 0L;
      while(stack.Count > 0)
      {
        score *= 5;
        switch(stack.Pop())
        {

          case '{':
            score += 3;
            break;
          case '(':
            score += 1;
            break;
          case '[':
            score += 2;
            break;
          case '<':
            score += 4;
            break;
        }
      }

      return score;
    }
  }
}
