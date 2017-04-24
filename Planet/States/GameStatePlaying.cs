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
    private EnemyManager enemyManager;
    private Player p1, p2;
    private Ship ship;
    private Ship ship2;

    public GameStatePlaying(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      world = new World();
      p1 = new Player(PlayerIndex.One);
      p2 = new Player(PlayerIndex.Two);

      //ship = new RewinderShip(new Vector2(500, 500), world);
      ship = new RewinderShip(new Vector2(500, 500), world);
      world.PostGameObj(ship);
      ship2 = new PossessorShip(new Vector2(1000, 500), world, p2);
      world.PostGameObj(ship2);

      p1.SetShip(ship);
      p2.SetShip(ship2);

      enemyManager = new EnemyManager(world);
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
      p1.Update(gameTime);
      p2.Update(gameTime);
      enemyManager.Update(gameTime);
      world.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      world.Draw(spriteBatch);
    }
  }
}
