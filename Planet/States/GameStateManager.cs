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
    public Player P1 { get; set; }
    public Player P2 { get; set; }
    private LinkedList<GameState> stateStack;
    public GameSettings Settings { get; private set; }

    public GameStateManager()
    {
      stateStack = new LinkedList<GameState>();
      Settings = new GameSettings();
      P1 = new Player(PlayerIndex.One);
      P2 = new Player(PlayerIndex.Two);
      Push(new GameStateTitleScreen(this));
    }
    public void Reset()
    {
      stateStack = new LinkedList<GameState>();
      Settings = new GameSettings();
      P1 = new Player(PlayerIndex.One);
      P2 = new Player(PlayerIndex.Two);
      Push(new GameStateTitleScreen(this));
    }
    public LinkedList<GameState> GetStates()
    {
      return stateStack;
    }
    public void ChangeState(GameState state)
    {
      Pop();
      Push(state);
    }
    public void Pop()
    {
      if (stateStack.Count() == 0)
        throw new Exception("Attempted to pop from an empty game stack");
      GameState popped = stateStack.First(); stateStack.RemoveFirst();
      popped.Leaving();
      if (stateStack.Count() != 0)
        stateStack.First().Revealed();
    }
    public void Push(GameState state)
    {
      if (stateStack.Count() != 0)
        stateStack.First().Obscuring();
      stateStack.AddFirst(state);
      state.Entered();
    }
    public GameState Peek()
    {
      if (stateStack.Count() == 0)
        return null;
      return stateStack.First();
    }
    public void Update(GameTime gt)
    {
      foreach (GameState state in stateStack.ToList())
      {
        if (state.UpdateEnabled)
          state.Update(gt);
      }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (GameState state in stateStack.Reverse())
      {
        if (state.DrawEnabled)
          state.Draw(spriteBatch);
      }
    }

  }
}
