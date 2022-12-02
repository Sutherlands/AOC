using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day2

  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      var scores = lines.Select(l =>
      {
        Hand you, opponent;
        switch (l[0])
        {
          case 'A':
            opponent = Hand.Rock;
            break;
          case 'B':
            opponent = Hand.Paper;
            break;
          case 'C':
            opponent = Hand.Scissors;
            break;
          default:
            throw new Exception();
        }

        switch (l[2])
        {
          case 'X':
            you = Hand.Rock;
            break;
          case 'Y':
            you = Hand.Paper;
            break;
          case 'Z':
            you = Hand.Scissors;
            break;
          default:
            throw new Exception();
        }
        return GetScore(you, opponent);
      });

      Console.WriteLine(scores.Sum());
    }

    public enum Hand { Rock = 0, Paper = 1, Scissors = 2}
    public static int GetScore(Hand you, Hand opponent)
    {
      var handResult = 0;
      if(you == opponent)
      {
        handResult += 3;
      }
      if((you - opponent + 3) % 3 == 1)
      {
        handResult += 6;
      }
      handResult += 1 + (int)you;
      return handResult;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      var scores = lines.Select(l =>
      {
        Hand you, opponent;
        switch (l[0])
        {
          case 'A':
            opponent = Hand.Rock;
            break;
          case 'B':
            opponent = Hand.Paper;
            break;
          case 'C':
            opponent = Hand.Scissors;
            break;
          default:
            throw new Exception();
        }

        switch (l[2])
        {
          case 'X': // lose
            you = (Hand)(((int)opponent + 2) % 3);
            break;
          case 'Y': // draw
            you = opponent;
            break;
          case 'Z': // win
            you = (Hand)(((int)opponent + 1) % 3);
            break;
          default:
            throw new Exception();
        }
        return GetScore(you, opponent);
      });

      Console.WriteLine(scores.Sum());
    }
  }
}
