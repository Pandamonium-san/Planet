using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class HighScoreEntry
  {
    public int Wave { get; set; }
    public int TotalScore { get; private set; }
    public string Name1 { get; set; }
    public int Score1 { get; private set; }
    public string Ship1 { get; set; }
    public string Name2 { get; set; }
    public int Score2 { get; private set; }
    public string Ship2 { get; set; }

    public HighScoreEntry(int wave, string name1, int score1, string ship1, string name2 = "", int score2 = 0, string ship2 = "")
    {
      TotalScore = score1 + score2;
      Wave = wave;
      Name1 = name1;
      Score1 = score1;
      Ship1 = ship1;
      Name2 = name2;
      Score2 = score2;
      Ship2 = ship2;
    }
    public class ByScore : IComparer<HighScoreEntry>
    {
      public int Compare(HighScoreEntry x, HighScoreEntry y)
      {
        return y.TotalScore.CompareTo(x.TotalScore);
      }
    }
  }
}
