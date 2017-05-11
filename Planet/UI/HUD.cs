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
    private Text waveCounter;

    public HUD(World world, Player p1, Player p2, EnemyManager em)
    {
      future18 = AssetManager.GetFont("future18");
      this.world = world;
      this.p1 = p1;
      this.p2 = p2;
      this.em = em;

      lifeBars = new LifeBar[4];
      lifeBars[0] = new LifeBar(p1.Ship, new Vector2(25, Game1.ScreenHeight - 50), 500, 30);
      lifeBars[1] = new LifeBar(p2.Ship, new Vector2(Game1.ScreenWidth - 25 - 500, Game1.ScreenHeight - 50), 500, 30, true);
      ability1 = new AbilityIcon(p1.Ship, new Vector2(585, Game1.ScreenHeight - 50));
      ability2 = new AbilityIcon(p2.Ship, new Vector2(Game1.ScreenWidth - 585, Game1.ScreenHeight - 50));
      score1 = new Text(future18, "test", new Vector2(25, 15), Color.White, Text.Align.Left);
      score2 = new Text(future18, "test", new Vector2(Game1.ScreenWidth - 25, 15), Color.White, Text.Align.Right);
      waveCounter = new Text(future18, "", new Vector2(Game1.ScreenWidth / 2, 50), Color.White);
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
          pos = lifeBars[0].Pos + new Vector2(5, -25);
          mirrored = false;
        }
        else
        {
          pos = lifeBars[1].Pos + new Vector2(500 - 200 - 5, -25);
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
