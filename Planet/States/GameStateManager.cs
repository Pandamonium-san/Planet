using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateManager
  {
    Stack<GameState> currentStates;
    public GameStateManager()
    {
      currentStates = new Stack<GameState>();
    }
    public void ChangeState(GameState state)
    {
      Pop();
      Push(state);
    }
    public GameState Pop()
    {
      if (currentStates.Count() == 0)
        throw new Exception("Attempted to pop from an empty game stack");
      GameState popped = currentStates.Pop();
      popped.Leaving();
      if (currentStates.Count() != 0)
        currentStates.Peek().Revealed();
      return popped;
    }
    public void Push(GameState state)
    {
      if (currentStates.Count() != 0)
        currentStates.Peek().Obscuring();
      currentStates.Push(state);
      state.Entered();
    }
    public GameState Peek()
    {
      if (currentStates.Count() == 0)
        return null;
      return currentStates.Peek();
    }
    public void Update(GameTime gt)
    {
      Peek().Update(gt);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      Peek().Draw(spriteBatch);
    }
  }
}
