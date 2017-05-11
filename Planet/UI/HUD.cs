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
    private EnemyManager em;
    private LifeBar[] lifeBars;
    private AbilityIcon ability1, ability2;
    private Text score1, score2;
    private Text wpn1, wpn2;
    private Text waveCounter;

    public HUD(World world, Player p1, Player p2, EnemyManager em)
    {
      future18 = AssetManager.GetFont("future18");
      this.world = world;
      this.p1 = p1;
      this.p2 = p2;
      this.em = em;

      lifeBars = new LifeBar[4];
      lifeBars[0] = new LifeBar(p1.Ship, new Vector2(25, Game1.ScreenHeight - 60), 500, 30);
      lifeBars[1] = new LifeBar(p2.Ship, new Vector2(Game1.ScreenWidth - 25 - 500, Game1.ScreenHeight - 60), 500, 30, true);
      ability1 = new AbilityIcon(p1.Ship, new Vector2(585, Game1.ScreenHeight - 60));
      ability2 = new AbilityIcon(p2.Ship, new Vector2(Game1.ScreenWidth - 585, Game1.ScreenHeight - 60));
      wpn1 = new Text(future18, "", lifeBars[0].Pos + new Vector2(5, -30), Color.White, Text.Align.Left);
      wpn2 = new Text(future18, "", lifeBars[1].Pos + new Vector2(500 - 5, -30), Color.White, Text.Align.Right);

      score1 = new Text(future18, "test", new Vector2(25, 15), Color.White, Text.Align.Left);
      score2 = new Text(future18, "test", new Vector2(Game1.ScreenWidth - 25, 15), Color.White, Text.Align.Right);
      waveCounter = new Text(future18, "", new Vector2(Game1.ScreenWidth / 2, 30), Color.White);
    }
    public void Update(GameTime gameTime)
    {
      MakePossessedShipLifeBar(p1);
      MakePossessedShipLifeBar(p2);
      for (int i = 0; i < lifeBars.Length; i++)
      {
        if (lifeBars[i] != null)
          lifeBars[i].Update();
      }

      ability1.Update();
      ability2.Update();
      score1.Set((((int)p1.Score / 10) * 10).ToString("D10"));
      score2.Set((((int)p2.Score / 10) * 10).ToString("D10"));
      if (p1.Ship is PossessorShip && ((PossessorShip)p1.Ship).PossessedShip != null)
        wpn1.Set(((PossessorShip)p1.Ship).PossessedShip.CurrentWeapon.Name);
      else
        wpn1.Set(p1.Ship.CurrentWeapon.Name);
      if (p2.Ship is PossessorShip && ((PossessorShip)p2.Ship).PossessedShip != null)
        wpn2.Set(((PossessorShip)p2.Ship).PossessedShip.CurrentWeapon.Name);
      else
        wpn2.Set(p2.Ship.CurrentWeapon.Name);
      waveCounter.Set("Wave " + em.WaveCounter.ToString());
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap);
      for (int i = 0; i < lifeBars.Length; i++)
      {
        if (lifeBars[i] != null)
          lifeBars[i].Draw(spriteBatch);
      }
      ability1.Draw(spriteBatch);
      ability2.Draw(spriteBatch);
      score1.Draw(spriteBatch);
      score2.Draw(spriteBatch);
      wpn1.Draw(spriteBatch);
      wpn2.Draw(spriteBatch);
      waveCounter.Draw(spriteBatch);
      spriteBatch.End();
    }
    private void MakePossessedShipLifeBar(Player p)
    {
      if (!(p.Ship is PossessorShip))
        return;
      LifeBar lb = null;
      PossessorShip ps = (PossessorShip)p.Ship;
      if (ps.PossessedShip != null)
      {
        Vector2 pos = Vector2.Zero;
        bool mirrored;
        if (p.Index == PlayerIndex.One)
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

      if (p.Index == PlayerIndex.One)
        lifeBars[2] = lb;
      else
        lifeBars[3] = lb;
    }
  }
}
