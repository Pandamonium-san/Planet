using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class TimeMachine
  {
    public static readonly int maxRewindableFrames = 180;                               // how many frames back we can go
    public static readonly int framesBetweenStates = 3;                                  // 1 = save every frame, 2 = save every other frame, 3 = every third frame, etc. Has a large effect on performance
    public static readonly int bufferSize = maxRewindableFrames / framesBetweenStates;   // how many frame states actually saved

    private GameObject parent;
    public bool isRewinding;
    public FixedList<GameObject.GOState> stateBuffer;
    public int remainingFramesToRewind;
    // lerp
    private GameObject.GOState lerpStart;
    private GameObject.GOState lerpTarget;
    private float lerpAmount;

    private FrameTimer rewindTimer;

    public TimeMachine(GameObject parent)
    {
      this.parent = parent;
      stateBuffer = new FixedList<GameObject.GOState>(bufferSize);
    }
    public void StartRewind(int x)
    {
      isRewinding = true;
      rewindTimer = new FrameTimer(x, () => isRewinding = false);
      rewindTimer.Start();
    }
    public void DoRewind()
    {
      if (rewindTimer.frames % framesBetweenStates == 0)
        LoadPreviousState();
      else
        LerpToPreviousState();

      rewindTimer.Update();
    }
    private void LerpToPreviousState()
    {
      GameObject.GOState data = stateBuffer.Peek();
      if (data != null)
      {
        if (lerpTarget != data)
        {
          lerpStart = parent.GetState();
          lerpTarget = data;
          lerpAmount = 1.0f / framesBetweenStates;
        }
        Vector2 newPos = new Vector2(
            MathHelper.Lerp(lerpStart.Pos.X, lerpTarget.Pos.X, lerpAmount),
            MathHelper.Lerp(lerpStart.Pos.Y, lerpTarget.Pos.Y, lerpAmount));
        parent.Pos = newPos;
        lerpAmount += 1.0f / framesBetweenStates;
      }
    }
    public void AddState(GameObject.GOState state)
    {
      stateBuffer.AddFirst(state);
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
          isRewinding = false;
        return;
      }
      parent.SetState(stateBuffer.Pop());
    }
  }
}
