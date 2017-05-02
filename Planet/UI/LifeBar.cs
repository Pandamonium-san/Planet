using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class LifeBar
  {
    private Ship ship;
    private ValueBar healthBar, shieldBar;

    public LifeBar(Ship ship, int x, int y, int width, int height)
    {
      this.ship = ship;
      shieldBar = new ValueBar(
        new Rectangle(x, y, width, height),
        ship.maxShield,
        ship.currentShield,
        AssetManager.GetTexture("blue_button05"),
        null);
      healthBar = new ValueBar(
        new Rectangle(x + 5, y + 5, width - 10, height - 10),
        ship.maxHealth,
        ship.currentHealth,
        AssetManager.GetTexture("green_button05"),
        AssetManager.GetTexture("grey_button05"));
    }
    public void Update(GameTime gameTime)
    {
      healthBar.Value = ship.currentHealth;
      shieldBar.Value = ship.currentShield;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      shieldBar.Draw(spriteBatch);
      healthBar.Draw(spriteBatch);
    }
  }
}
