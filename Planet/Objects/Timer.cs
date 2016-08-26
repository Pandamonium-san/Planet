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
    public double secondsToActivate { get; private set; }
    public double elapsedSeconds { get; private set; }
    public double Fraction { get { return elapsedSeconds / secondsToActivate; } }

    private Action action;

    public Timer(double timeInSeconds, Action action = null, bool start = true) : this()
    {
      this.secondsToActivate = timeInSeconds;
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
      if (elapsedSeconds >= secondsToActivate)
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
    public double Fraction { get { return (double)frames / (double)framesToActivate; } }

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
