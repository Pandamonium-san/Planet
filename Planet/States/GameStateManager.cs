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
    Stack<GameState> stateStack;

    public GameStateManager()
    {
      stateStack = new Stack<GameState>();
    }
    public void ChangeState(GameState state)
    {
      Pop();
      Push(state);
    }
    public GameState Pop()
    {
      if (stateStack.Count() == 0)
        throw new Exception("Attempted to pop from an empty game stack");
      GameState popped = stateStack.Pop();
      popped.Leaving();
      if (stateStack.Count() != 0)
        stateStack.Peek().Revealed();
      return popped;
    }
    public void Push(GameState state)
    {
      if (stateStack.Count() != 0)
        stateStack.Peek().Obscuring();
      stateStack.Push(state);
      state.Entered();
    }
    public GameState Peek()
    {
      if (stateStack.Count() == 0)
        return null;
      return stateStack.Peek();
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
