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
    private LifeBar lifeBar1, lifeBar2;

    public HUD(World world, Player p1, Player p2)
    {
      this.world = world;
      this.p1 = p1;
      this.p2 = p2;

      lifeBar1 = new LifeBar(p1.Ship, 45, 45, 310, 30);
      lifeBar2 = new LifeBar(p2.Ship, Game1.ScreenWidth - 355, 45, 310, 30);
    }
    public void Update(GameTime gameTime)
    {
      lifeBar1.Update(gameTime);
      lifeBar2.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      lifeBar1.Draw(spriteBatch);
      lifeBar2.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
