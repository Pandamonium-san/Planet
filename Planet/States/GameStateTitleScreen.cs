using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateTitleScreen : GameState
  {
    private GameStateManager gsm;
    private Menu menu;
    private MenuController mc;
    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      menu = new Menu();
      mc = new MenuController(PlayerIndex.One, menu);
    }
    public override void Entered()
    {

    }
    public override void Leaving()
    {

    }
    public override void Revealed()
    {

    }
    public override void Obscuring()
    {

    }
    public override void Update(GameTime gameTime)
    {
      if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start, false) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Start, true))
      {
        gsm.Pop();
      }
      menu.Update(gameTime);
      mc.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      menu.Draw(spriteBatch);
    }
  }
}
