using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public enum CommandType
  {
    Move, // x,y direction
    MoveTo, // x,y coordinates
    Fire,
    LookAtPoint, // x,y coordinates
    LookAtTarget,
    Rotate, // x degrees
    SetVelocity,
    AddVelocity
  }
  public struct Command
  {
    public CommandType type;
    public float x;
    public float y;
    public int startFrame;
    public int endFrame;
    public Command(CommandType type, float x, float y, int startFrame, int endFrame)
    {
      this.type = type;
      this.x = x;
      this.y = y;
      this.startFrame = startFrame;
      this.endFrame = endFrame;
    }
  }
}
