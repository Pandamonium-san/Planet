using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStatePlaying : GameState
  {
    private GameStateManager gsm;
    private World world;
    private Player p1, p2;
    private RewinderShip ship;
    private BlinkerShip ship2;

    public GameStatePlaying(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      world = new World();

      ship = new RewinderShip(new Vector2(500, 500), world);
      world.PostGameObj(ship);
      ship2 = new BlinkerShip(new Vector2(1000, 500), world);
      world.PostGameObj(ship2);

      p1 = new Player(PlayerIndex.One);
      p2 = new Player(PlayerIndex.Two);
      p1.SetShip(ship);
      p2.SetShip(ship2);
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
        gsm.Push(new GameStateTitleScreen(gsm));
      }
      p1.Update(gameTime);
      p2.Update(gameTime);
      world.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      world.Draw(spriteBatch);
    }
  }
}
