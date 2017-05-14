using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Planet
{
  class HighScoreList
  {
    public readonly int SavedEntries = 50;
    public HighScoreEntry Last { get { return scores.Last(); } }
    public int Count { get { return scores.Count; } }
    public HighScoreEntry MostRecent { get; private set; }
    public HighScoreEntry this[int i]
    {
      get
      {
        return scores[i];
      }
      set
      {
        scores[i] = value;
      }
    }

    private readonly string savePath = "highscores.txt";
    List<HighScoreEntry> scores;

    public HighScoreList()
    {
      scores = new List<HighScoreEntry>();
    }
    public void AddEntry(HighScoreEntry entry)
    {
      scores.Add(entry);
      scores.Sort(new HighScoreEntry.ByScore());
      MostRecent = entry;
    }
    public void SaveHighScores()
    {
      using (StreamWriter writer = new StreamWriter(savePath))
      {
        foreach (HighScoreEntry entry in scores)
        {
          string data = entry.Name1 + "\t" +  //0
            entry.Name2 + "\t" +              //1
            entry.Wave + "\t" +               //2
            entry.Score1 + "\t" +             //3
            entry.Score2 + "\t" +             //4
            entry.Ship1 + "\t" +              //5
            entry.Ship2;                      //6
          writer.WriteLine(data);
        }
      }
    }
    public void LoadHighScores()
    {
      scores = new List<HighScoreEntry>();
      if (!File.Exists(savePath))
        return;
      using (StreamReader reader = new StreamReader(savePath))
      {
        for (int i = 0; i < SavedEntries && !reader.EndOfStream; i++)
        {
          string data = reader.ReadLine();
          string[] split = data.Split('\t');
          HighScoreEntry entry = new HighScoreEntry(
            int.Parse(split[2]),
            split[0],
            int.Parse(split[3]),
            split[5],
            split[1],
            int.Parse(split[4]),
            split[6]);
          AddEntry(entry);
        }
      }
    }
  }
}
