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
    private Sprite overlay;

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

      overlay = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), AssetManager.Pixel);
      overlay.Scale = 2000;
      overlay.color = Color.Black;
      overlay.alpha = 1.0f;
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
      base.Update(gameTime);
      foreach (PlayerShipController pc in pcs)
      {
        pc.Update(gameTime);
      }
      enemyManager.Update(gameTime);
      world.Update(gameTime);
      hud.Update(gameTime);

      if (enemyManager.WaveDefeated())
      {
        enemyManager.SendNextWave(5.5f);
        hud.FlashWaveText(5.0f);
        float hp = GetHighestHealthPercentage();
        foreach (PlayerShipController pc in pcs)
        {
          pc.Player.Score += enemyManager.WaveCounter * 2000 / pcs.Count;
          if (pc.Ship.Disposed)
            RespawnShip(pc, hp);
        }
      }

      if (fadeTimer.Counting || fadeTimer.Finished)
        return;
      if (PlayersAreDead())
      {
        gsm.Push(new GameStateInputName(gsm, enemyManager.WaveCounter));
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

      spriteBatch.Begin();
      if (fadeTimer.Counting)
        overlay.Draw(spriteBatch, a);
      spriteBatch.End();
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
      if (PlayersAreDead())
      {
        FadeTransition(2.0f, gsm.Reset, true);
      }
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
    }
    private bool PlayersAreDead()
    {
      foreach (PlayerShipController pc in pcs)
      {
        if (!pc.Ship.Disposed)
          return false;
      }
      return true;
    }
    private float GetHighestHealthPercentage()
    {
      float highest = -1;
      foreach (PlayerShipController pc in pcs)
      {
        float percentage = pc.Ship.currentHealth / pc.Ship.maxHealth;
        if (percentage > highest)
        {
          highest = percentage;
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
      pc.Player.Score -= 30000;
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
