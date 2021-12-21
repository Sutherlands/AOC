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
  public static class Day21
  {
    public static int DieRoll = 0;
    public static int DieRollCount = 0;

    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay21.txt").ToList();
      var player1Position = int.Parse(lines[0].Split(": ")[1]);
      var player2Position = int.Parse(lines[1].Split(": ")[1]);

      var player1 = new Player { Position = player1Position, Score = 0 };
      var player2 = new Player { Position = player2Position, Score = 0 };

      var nextPlayer = player1;

      while(player1.Score < 1000 && player2.Score < 1000)
      {
        var movementSpaces = GetDieRoll() + GetDieRoll() + GetDieRoll();
        nextPlayer.Position = (nextPlayer.Position + movementSpaces) % 10;
        nextPlayer.Score += nextPlayer.Position == 0 ? 10 : nextPlayer.Position;

        nextPlayer = nextPlayer == player1 ? player2 : player1;
      }

      var losingScore = player1.Score < 1000 ? player1.Score : player2.Score;
      Console.WriteLine(losingScore * DieRollCount);
    }

    public class Player
    {
      public int Position { get; set; }
      public int Score { get; set; }
    }

    public static int GetDieRoll()
    {
      DieRollCount++;
      DieRoll = (DieRoll + 1) % 100;
      return DieRoll;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay21.txt").ToList();
      var player1Position = int.Parse(lines[0].Split(": ")[1]);
      var player2Position = int.Parse(lines[1].Split(": ")[1]);

      var results = GetResults(player1Position, player2Position, 0, 0, true);
      
    }

    public class QuantumResult
    {
      public long NumberWinsPlayer1 { get; set; }
      public long NumberWinsPlayer2 { get; set; }
    }

    public static Dictionary<int, QuantumResult> ResultCache = new Dictionary<int, QuantumResult>();

    public static QuantumResult GetResults(int player1Position, int player2Position, int player1Score, int player2Score, bool isPlayerOnesTurn)
    {
      const int targetScore = 21;

      var key = player1Position * 100 + player2Position * 10000 + player1Score * 1000000 + player2Score * 100000000 + (isPlayerOnesTurn ? 1 : 2);
      QuantumResult result;
      if (!ResultCache.TryGetValue(key, out result))
      {
        //Console.WriteLine($"Checking {player1Position} ({player1Score}), {player2Position} ({player2Score})");
        if(player1Score >= targetScore)
        {
          result = new QuantumResult { NumberWinsPlayer1 = 1 };
        }
        else if(player2Score >= targetScore)
        {
          result = new QuantumResult { NumberWinsPlayer2 = 1 };
        }
        else if (isPlayerOnesTurn)
        {
          var results = GetNewPositions(player1Position).Select(newP1Position => GetResults(newP1Position, player2Position, GetNewScore(player1Score, newP1Position), player2Score, false)).ToList();
          result = new QuantumResult
          {
            NumberWinsPlayer1 = results.Sum(r => r.NumberWinsPlayer1),
            NumberWinsPlayer2 = results.Sum(r => r.NumberWinsPlayer2)
          };
        }
        else
        {
          var results = GetNewPositions(player2Position).Select(newP2Position => GetResults(player1Position, newP2Position, player1Score, GetNewScore(player2Score, newP2Position), true)).ToList();
          result = new QuantumResult
          {
            NumberWinsPlayer1 = results.Sum(r => r.NumberWinsPlayer1),
            NumberWinsPlayer2 = results.Sum(r => r.NumberWinsPlayer2)
          };
        }

        ResultCache[key] = result;
      }
      return result;
    }

    private static IEnumerable<int> GetNewPositions(int currentPosition)
    {
      for(int die1 = 0; die1 < 3; ++die1)
      {
        for (int die2 = 0; die2 < 3; ++die2)
        {
          for (int die3 = 0; die3 < 3; ++die3)
          {
            yield return (currentPosition + die1 + die2 + die3 + 3) % 10;
          }
        }
      }
    }

    private static int GetNewScore(int player1Score, int position)
    {
      return player1Score + (position == 0 ? 10 : position);
    }
  }
}
