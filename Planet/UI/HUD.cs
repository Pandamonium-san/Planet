using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class HUD
  {
    private SpriteFont future18;
    private World world;
    private Player p1, p2;
    private PlayerShipController pc1, pc2;
    private EnemyManager em;
    private LifeBar[] lifeBars;
    private AbilityIcon ability1, ability2;

    private Score score1, score2;

    private Text wpn1, wpn2;
    private Text waveCounter;
    private Timer waveTextFlash;

    public HUD(World world, EnemyManager em)
    {
      future18 = AssetManager.GetFont("future18");
      this.world = world;
      this.em = em;
      lifeBars = new LifeBar[4];
      waveCounter = new Text(future18, "", new Vector2(Game1.ScreenWidth / 2, 35), Color.White);
      waveTextFlash = new Timer(0, null, false);
    }
    public void BuildHUD(Player player, PlayerShipController playerController)
    {
      if (player.Index == PlayerIndex.One)
      {
        this.p1 = player;
        this.pc1 = playerController;
        lifeBars[0] = new LifeBar(playerController.Ship, new Vector2(30, Game1.ScreenHeight - 60), 500, 30);
        ability1 = new AbilityIcon(playerController.Ship, new Vector2(590, Game1.ScreenHeight - 60));
        wpn1 = new Text(future18, "", lifeBars[0].Pos + new Vector2(5, -30), Color.White, Text.Align.Left);
        score1 = new Score(p1, new Text(future18, "test", new Vector2(25, 20), Color.White, Text.Align.Left));
      }
      else if (player.Index == PlayerIndex.Two)
      {
        this.p2 = player;
        this.pc2 = playerController;
        lifeBars[1] = new LifeBar(pc2.Ship, new Vector2(Game1.ScreenWidth - 30 - 500, Game1.ScreenHeight - 60), 500, 30, true);
        ability2 = new AbilityIcon(pc2.Ship, new Vector2(Game1.ScreenWidth - 590, Game1.ScreenHeight - 60));
        wpn2 = new Text(future18, "", lifeBars[1].Pos + new Vector2(500 - 5, -30), Color.White, Text.Align.Right);
        score2 = new Score(p2, new Text(future18, "test", new Vector2(Game1.ScreenWidth - 25, 20), Color.White, Text.Align.Right));
      }
    }
    public void Update(GameTime gameTime)
    {
      if (p1 != null)
      {
        MakePossessedShipLifeBar(pc1);
        ability1.Update();
        score1.Update(gameTime);
        if (pc1.Ship is PossessorShip && ((PossessorShip)pc1.Ship).PossessedShip != null)
          wpn1.Set(((PossessorShip)pc1.Ship).PossessedShip.CurrentWeapon.Name);
        else
          wpn1.Set(pc1.Ship.CurrentWeapon.Name);
      }
      if (p2 != null)
      {
        MakePossessedShipLifeBar(pc2);
        ability2.Update();
        score2.Update(gameTime);
        if (pc2.Ship is PossessorShip && ((PossessorShip)pc2.Ship).PossessedShip != null)
          wpn2.Set(((PossessorShip)pc2.Ship).PossessedShip.CurrentWeapon.Name);
        else
          wpn2.Set(pc2.Ship.CurrentWeapon.Name);
      }

      for (int i = 0; i < lifeBars.Length; i++)
      {
        if (lifeBars[i] != null)
          lifeBars[i].Update();
      }

      waveCounter.Set("Wave " + em.WaveCounter.ToString());
      if (waveTextFlash.Counting)
      {
        waveCounter.Scale = 1.4f + (float)Math.Sin(waveTextFlash.elapsedSeconds * Math.PI * 2 - Math.PI / 2) * 0.4f;
        waveCounter.alpha = 0.80f + (float)Math.Sin(waveTextFlash.elapsedSeconds * Math.PI * 2 - Math.PI / 2) * 0.20f;
      }
      else
      {
        waveCounter.Scale = 1.0f;
        waveCounter.alpha = 0.7f;
      }
      waveTextFlash.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap);
      for (int i = 0; i < lifeBars.Length; i++)
      {
        if (lifeBars[i] != null)
          lifeBars[i].Draw(spriteBatch);
      }
      if (p1 != null)
      {
        ability1.Draw(spriteBatch);
        score1.Draw(spriteBatch);
        wpn1.Draw(spriteBatch);
      }
      if (p2 != null)
      {
        ability2.Draw(spriteBatch);
        score2.Draw(spriteBatch);
        wpn2.Draw(spriteBatch);
      }

      waveCounter.Draw(spriteBatch);
      spriteBatch.End();
    }
    public void FlashWaveText(double seconds)
    {
      waveTextFlash.Start(seconds);
    }
    private void MakePossessedShipLifeBar(PlayerShipController pc)
    {
      if (!(pc.Ship is PossessorShip))
        return;
      LifeBar lb = null;
      PossessorShip ps = (PossessorShip)pc.GetShip();
      if (ps.PossessedShip != null)
      {
        Vector2 pos = Vector2.Zero;
        bool mirrored;
        if (pc.Player.Index == PlayerIndex.One)
        {
          pos = lifeBars[0].Pos + new Vector2(275, -20);
          mirrored = false;
        }
        else
        {
          pos = lifeBars[1].Pos + new Vector2(500 - 200 - 275, -20);
          mirrored = true;
        }
        lb = new LifeBar(ps.PossessedShip, pos, 200, 15, mirrored, 2);
      }

      if (pc.Player.Index == PlayerIndex.One)
        lifeBars[2] = lb;
      else
        lifeBars[3] = lb;
    }
  }
}
