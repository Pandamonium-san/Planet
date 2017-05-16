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
        AssetManager.GetTexture("blueButton05"),
        null,
        mirrored);
      healthBar = new ValueBar(
        new Vector2(shieldWidth, shieldWidth),
        width - shieldWidth * 2, 
        height - shieldWidth * 2,
        ship.maxHealth,
        AssetManager.GetTexture("greenButton05"),
        AssetManager.GetTexture("greyButton05"),
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
    public void SetShip(Ship ship)
    {
      this.ship = ship;
    }
  }
}
