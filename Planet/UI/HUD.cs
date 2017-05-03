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
    private World world;
    private Player p1, p2;
    private LifeBar[] lifeBars;
    private LifeBar lifeBar1, lifeBar2;
    private LifeBar lifeBar3, lifeBar4;
    private AbilityIcon ability1, ability2;

    public HUD(World world, Player p1, Player p2)
    {
      this.world = world;
      this.p1 = p1;
      this.p2 = p2;

      lifeBars = new LifeBar[4];

      ability1 = new AbilityIcon(p1.Ship, new Vector2(50, Game1.ScreenHeight - 700));
      ability2 = new AbilityIcon(p2.Ship, new Vector2(Game1.ScreenWidth - 150, Game1.ScreenHeight - 700));
      lifeBar1 = new LifeBar(p1.Ship, 45, 45, 310, 30);
      lifeBar2 = new LifeBar(p2.Ship, Game1.ScreenWidth - 355, 45, 310, 30, true);
    }
    public void Update(GameTime gameTime)
    {
      lifeBar1.Update();
      lifeBar2.Update();
      if (p1.Ship is PossessorShip)
      {
        PossessorShip ps = (PossessorShip)p1.Ship;
        if (ps.PossessedShip != null)
          lifeBar3 = new LifeBar(ps.PossessedShip, 45, 85, 150, 10, false, 2);
      }
      ability1.Update();
      ability2.Update();
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      lifeBar1.Draw(spriteBatch);
      lifeBar2.Draw(spriteBatch);
      if (lifeBar3 != null && lifeBar3.Ship != null && !lifeBar3.Ship.Disposed)
        lifeBar3.Draw(spriteBatch);
      ability1.Draw(spriteBatch);
      ability2.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
