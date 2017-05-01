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
    private HealthBar hb1, hb2;

    public HUD(World world, Player p1, Player p2)
    {
      this.world = world;
      this.p1 = p1;
      this.p2 = p2;
      hb1 = new HealthBar(new Rectangle(50, 50, 300, 30), p1.Ship.maxHealth, p1.Ship.currentHealth, Color.White, Color.White);
      hb2 = new HealthBar(new Rectangle(Game1.ScreenWidth - 350, 50, 300, 30), p2.Ship.maxHealth, p2.Ship.currentHealth, Color.White, Color.White, true);
    }
    public void Update(GameTime gameTime)
    {

      hb1.Value = p1.Ship.currentHealth;
      hb2.Value = p2.Ship.currentHealth;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      hb1.Draw(spriteBatch);
      hb2.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
