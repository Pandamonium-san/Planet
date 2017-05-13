using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStatePlaying : GameState, IJoinable
  {
    private GameStateManager gsm;
    private World world;
    private HUD hud;
    private EnemyManager enemyManager;
    private List<PlayerShipController> pcs;

    public GameStatePlaying(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      pcs = new List<PlayerShipController>();
      world = new World();
      enemyManager = new EnemyManager(world);
      hud = new HUD(world, enemyManager);

      if (gsm.P1.Joined)
        Join(gsm.P1);
      if (gsm.P2.Joined)
        Join(gsm.P2);
    }
    public void Join(Player player)
    {
      PlayerShipController pc = new PlayerShipController(player, CreatePlayerShip(player));
      hud.BuildHUD(player, (PlayerShipController)player.Controller);
      if (player.Index == PlayerIndex.One)
        pc.Ship.Pos = new Vector2(Game1.ScreenWidth * 0.33f, 700);
      else
        pc.Ship.Pos = new Vector2(Game1.ScreenWidth * 0.66f, 700);
      if (pcs.Count != 0)
        pc.Ship.currentHealth *= GetHighestHealthPercentage();
      pcs.Add(pc);
    }
    public override void Update(GameTime gameTime)
    {
      foreach (PlayerShipController pc in pcs)
      {
        pc.Update(gameTime);
      }
      enemyManager.Update(gameTime);
      world.Update(gameTime);
      hud.Update(gameTime);

      foreach (PlayerShipController pc in pcs)
      {
        if (!pc.Ship.Disposed)
          break;
        //game over
      }
      if (enemyManager.WaveDefeated())
      {
        enemyManager.SendNextWave(5.5f);
        hud.FlashWaveText(5.0f);
        float hp = GetHighestHealthPercentage();
        foreach (PlayerShipController pc in pcs)
        {
          if (pc.Ship.Disposed)
            RespawnShip(pc, hp);
        }
      }
      if ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start, false) &&
        InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Start, true) &&
        gsm.P1.Joined) ||
        (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Start, false) &&
        InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Start, true) &&
        gsm.P2.Joined))
      {
        gsm.Push(new GameStatePaused(gsm));
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      world.Draw(spriteBatch);
      hud.Draw(spriteBatch);
    }
    public override void Entered()
    {
    }
    public override void Leaving()
    {

    }
    public override void Revealed()
    {
      UpdateEnabled = true;
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
    }
    private float GetHighestHealthPercentage()
    {
      //Ship healthiest = null;
      float highest = -1;
      foreach (PlayerShipController pc in pcs)
      {
        float percentage = pc.Ship.currentHealth / pc.Ship.maxHealth;
        if (percentage > highest)
        {
          highest = percentage;
          //healthiest = pc.Ship;
        }
      }
      return highest;
    }
    private void RespawnShip(PlayerShipController pc, float healthPercentage = 1.0f)
    {
      pc.Ship.IsActive = true;
      pc.Ship.Disposed = false;
      pc.Ship.Pos = new Vector2(Game1.ScreenWidth * 0.5f, 700);
      pc.Ship.currentHealth = pc.Ship.maxHealth * healthPercentage;
      pc.Ship.Flash(2.0f, Color.White, false);
      pc.Ship.MakeInvulnerable(2.0f);
      pc.Player.Score -= 25000;
      pc.SetShip(pc.Ship);
      world.PostGameObj(pc.Ship);
    }
    private Ship CreatePlayerShip(Player p)
    {
      Ship ship;
      switch (p.SelectedShip)
      {
        case "Rewinder":
          ship = new RewinderShip(Vector2.Zero, world, p);
          break;
        case "Blinker":
          ship = new BlinkerShip(Vector2.Zero, world, p);
          break;
        case "Possessor":
          ship = new PossessorShip(Vector2.Zero, world, p);
          break;
        default:
          throw new Exception("Ship type '" + p.SelectedShip + "' is not implemented");
      }
      world.PostGameObj(ship);
      return ship;
    }
  }
}
