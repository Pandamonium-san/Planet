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
    private GameSettings gameSettings;
    private World world;
    private HUD hud;
    private EnemyManager enemyManager;
    private Player p1, p2;

    public GameStatePlaying(GameStateManager gameStateManager, GameSettings gameSettings)
    {
      gsm = gameStateManager;
      this.gameSettings = gameSettings;
      world = new World();
      p1 = new Player(PlayerIndex.One);
      p2 = new Player(PlayerIndex.Two);
      SetPlayerShip(p1).Pos = new Vector2(500, 500);
      SetPlayerShip(p2).Pos = new Vector2(1000, 500);
      enemyManager = new EnemyManager(world);
      hud = new HUD(world, p1, p2);
    }
    Ship SetPlayerShip(Player p)
    {
      Ship ship = null;
      if (p.Index == PlayerIndex.One)
        ship = InstantiateShip(gameSettings.p1StarterShip, p);
      else if (p.Index == PlayerIndex.Two)
        ship = InstantiateShip(gameSettings.p2StarterShip, p);
      p.SetShip(ship);
      world.PostGameObj(ship);
      return ship;
    }
    Ship InstantiateShip(string shipType, Player p)
    {
      switch (shipType)
      {
        case "RewinderShip":
          return new RewinderShip(Vector2.Zero, world);
        case "BlinkerShip":
          return new BlinkerShip(Vector2.Zero, world);
        case "PossessorShip":
          return new PossessorShip(Vector2.Zero, world, p);
      }
      throw new Exception("Ship type '" + shipType + "' is not implemented");
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
      hud.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      world.Draw(spriteBatch);
      hud.Draw(spriteBatch);
    }
  }
}
