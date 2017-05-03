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
    public Ship Ship { get { return ship; } }
    private Ship ship;
    private ValueBar healthBar, shieldBar;

    public LifeBar(Ship ship, int x, int y, int width, int height, bool mirrored = false, int shieldWidth = 4)
    {
      this.ship = ship;
      shieldBar = new ValueBar(
        new Rectangle(x, y, width, height),
        ship.maxShield,
        ship.currentShield,
        AssetManager.GetTexture("blue_button05"),
        null,
        mirrored);
      healthBar = new ValueBar(
        new Rectangle(x + shieldWidth, y + shieldWidth, width - shieldWidth * 2, height - shieldWidth * 2),
        ship.maxHealth,
        ship.currentHealth,
        AssetManager.GetTexture("green_button05"),
        AssetManager.GetTexture("grey_button05"),
        mirrored);
    }
    public void Update()
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
