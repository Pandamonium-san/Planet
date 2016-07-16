using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class TimeMachine
  {
    GameObject parent;
    public static readonly int maxRewindableFrames = 180;                               // how many frames back we can go
    public static readonly int framesToSkipSaving = 5;                                  // 1 = save every frame, 2 = save every other frame, 3 = every third frame, etc. Has a large effect on performance
    public static readonly int bufferSize = maxRewindableFrames / framesToSkipSaving;   // how many frame states actually saved
    public FixedList<GameObject.State> stateBuffer;
    public int remainingFramesToRewind;
    // lerp
    private GameObject.State lerpStart;
    private GameObject.State lerpTarget;
    private float lerpTime;

    public TimeMachine(GameObject parent)
    {
      this.parent = parent;
      stateBuffer = new FixedList<GameObject.State>(bufferSize);
    }

    public void DoRewind()
    {
      if (remainingFramesToRewind % framesToSkipSaving == 0)
        LoadPreviousState();
      else
        LerpToPreviousState();

      if (--remainingFramesToRewind <= 0)
        parent.isRewinding = false;
    }
    private void LerpToPreviousState()
    {
      GameObject.State data = stateBuffer.Peek();
      if (data != null)
      {
        if (lerpTarget != data)
        {
          lerpStart = parent.GetState();
          lerpTarget = data;
          lerpTime = 1.0f / framesToSkipSaving;
        }

        Vector2 newPos = new Vector2(
            MathHelper.Lerp(lerpStart.Pos.X, lerpTarget.Pos.X, lerpTime),
            MathHelper.Lerp(lerpStart.Pos.Y, lerpTarget.Pos.Y, lerpTime));
        parent.Pos = newPos;
        lerpTime += 1.0f / framesToSkipSaving;
      }
    }
    public void SaveCurrentState()
    {
      stateBuffer.AddFirst(parent.GetState());
    }
    protected void LoadPreviousState()
    {
      // end of queue has been reached
      if (stateBuffer.Count <= 0)
      {
        // remove object if trying to rewind to before object was created
        if (parent.frame == 0)
          parent.disposed = true;
        else //if (remainingFramesToRewind == 0)
          parent.isRewinding = false;
        return;
      }
      parent.SetState(stateBuffer.Pop());
    }
  }
}
