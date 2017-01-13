using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public struct Timer
  {
    public bool counting { get; private set; }
    public double seconds { get; private set; }
    public double elapsedSeconds { get; private set; }
    public double Fraction { get { return elapsedSeconds / seconds; } }

    private Action action;

    /// <param name="timeInSeconds">Time before action is invoked.</param>
    /// <param name="action">Action to invoke after set amount of time.</param>
    /// <param name="start">Start the timer immediately without calling Start.</param>
    public Timer(double timeInSeconds, Action action = null, bool start = true) : this()
    {
      this.seconds = timeInSeconds;
      this.action = action;
      this.elapsedSeconds = 0;
      counting = start;
    }
    public void Start()
    {
      counting = true;
      elapsedSeconds = 0;
    }
    public void Update(GameTime gt)
    {
      if (!counting)
        return;
      elapsedSeconds += gt.ElapsedGameTime.TotalSeconds;
      if (elapsedSeconds >= seconds)
      {
        counting = false;
        if (action != null)
          action.Invoke();
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
