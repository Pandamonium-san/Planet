using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class LifeBar : Transform
  {
    public Ship Ship { get { return ship; } }
    private Ship ship;
    private ValueBar healthBar, shieldBar;

    public LifeBar(Ship ship, Vector2 pos, int width, int height, bool mirrored = false, int shieldWidth = 4)
      : base(Vector2.Zero)
    {
      this.ship = ship;
      shieldBar = new ValueBar(
        Vector2.Zero,
        width,
        height,
        ship.maxShield,
        AssetManager.GetTexture("blue_button05"),
        null,
        mirrored);
      healthBar = new ValueBar(
        new Vector2(shieldWidth, shieldWidth),
        width - shieldWidth * 2, 
        height - shieldWidth * 2,
        ship.maxHealth,
        AssetManager.GetTexture("green_button05"),
        AssetManager.GetTexture("grey_button05"),
        mirrored);
      shieldBar.Parent = this;
      healthBar.Parent = this;
      this.Pos = pos;
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
