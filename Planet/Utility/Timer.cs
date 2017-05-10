using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Timer
  {
    public double Fraction { get { return elapsedSeconds / seconds; } }
    public double Remaining { get { return seconds - elapsedSeconds; } }

    public bool Finished { get; private set; }
    public bool Counting { get; private set; }
    public bool Repeats { get; set; }
    public double seconds { get; private set; }
    public double elapsedSeconds { get; private set; }

    private Action action;

    /// <param name="timeInSeconds">Time before action is invoked.</param>
    /// <param name="action">Action to invoke after set amount of time.</param>
    /// <param name="start">Start the timer immediately without calling Start.</param>
    public Timer(double timeInSeconds, Action action = null, bool start = true, bool repeat = false)
    {
      this.seconds = timeInSeconds;
      this.action = action;
      this.elapsedSeconds = 0;
      Counting = start;
      Finished = false;
      Repeats = repeat;
    }
    public Timer(Timer other)
    {
      this.seconds = other.seconds;
      this.action = other.action;
      this.elapsedSeconds = other.elapsedSeconds;
      Counting = other.Counting;
      Finished = other.Finished;
      Repeats = other.Repeats;
    }
    public void ForceFinish()
    {
      elapsedSeconds = seconds;
    }
    public void Start(double seconds = -1)
    {
      Counting = true;
      Finished = false;
      if (seconds > 0)
        this.seconds = seconds;
      elapsedSeconds = 0;
    }
    public void Update(GameTime gt)
    {
      if (!Counting)
        return;
      elapsedSeconds += gt.ElapsedGameTime.TotalSeconds;
      if (elapsedSeconds >= seconds)
      {
        Counting = false;
        Finished = true;
        if (action != null)
          action.Invoke();
        if (Repeats)
          Start();
      }
    }
  }

  public struct FrameTimer
  {
    public bool counting { get; private set; }
    public int frames { get; private set; }
    public int framesToActivate { get; private set; }
    public double Fraction { get { if (framesToActivate == 0) return 1; else return (double)frames / (double)framesToActivate; } }

    private Action action;

    public FrameTimer(int frames, Action action = null, bool start = true) : this()
    {
      this.framesToActivate = frames;
      this.action = action;
      this.frames = 0;
      counting = start;
    }
    public void Start()
    {
      counting = true;
      frames = 0;
    }
    public void Update()
    {
      if (!counting)
        return;

      frames++;
      if (frames >= framesToActivate)
      {
        counting = false;
        if (action != null)
          action.Invoke();
      }
    }
  }
}
