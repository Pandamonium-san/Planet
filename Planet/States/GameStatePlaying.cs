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
      SetPlayerShip(p1).Pos = new Vector2(Game1.ScreenWidth * 0.33f, 700);
      SetPlayerShip(p2).Pos = new Vector2(Game1.ScreenWidth * 0.66f, 700);
      enemyManager = new EnemyManager(world);
      hud = new HUD(world, p1, p2, enemyManager);
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

      if (p1.Ship.Disposed && p2.Ship.Disposed)
      {
        // game over
        return;
      }
      if (enemyManager.WaveDefeated())
      {
        enemyManager.SendNextWave(5.5f);
        hud.FlashWaveText(5.0f);
        if (p1.Ship.Disposed)
          RespawnShip(p1, p2.Ship.currentHealth / p2.Ship.maxHealth * 0.5f);
        if (p2.Ship.Disposed)
          RespawnShip(p2, p1.Ship.currentHealth / p1.Ship.maxHealth * 0.5f);
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      world.Draw(spriteBatch);
      hud.Draw(spriteBatch);
    }
    public override void Entered()
    {
      AudioManager.PlayBgm("Kubbi - Firelight", 0.02f);
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
    private void RespawnShip(Player p, float healthPercentage = 1.0f)
    {
      p.Ship.IsActive = true;
      p.Ship.Disposed = false;
      p.Ship.Pos = new Vector2(Game1.ScreenWidth * 0.5f, 700);
      p.Ship.currentHealth = p.Ship.maxHealth * healthPercentage;
      p.Ship.Flash(2.0f, Color.White, false);
      p.Ship.MakeInvulnerable(2.0f);
      p.Score -= 25000;
      p.SetShip(p.Ship);
      world.PostGameObj(p.Ship);
    }
    private Ship SetPlayerShip(Player p)
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
    private Ship InstantiateShip(string shipType, Player p)
    {
      switch (shipType)
      {
        case "RewinderShip":
          return new RewinderShip(Vector2.Zero, world, p);
        case "BlinkerShip":
          return new BlinkerShip(Vector2.Zero, world, p);
        case "PossessorShip":
          return new PossessorShip(Vector2.Zero, world, p);
      }
      throw new Exception("Ship type '" + shipType + "' is not implemented");
    }
  }
}
