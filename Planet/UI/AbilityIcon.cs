using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class AbilityIcon : Transform
  {
    private double value;
    private Ship ship;
    Sprite back;
    Sprite icon;
    SpriteFont font;
    public AbilityIcon(Ship ship, Vector2 pos)
      : base(Vector2.Zero)
    {
      this.ship = ship;
      font = AssetManager.GetFont("future18");
      Texture2D iconTex = null;
      if (ship is RewinderShip)
        iconTex = AssetManager.GetTexture("rewind");
      else if (ship is BlinkerShip)
        iconTex = AssetManager.GetTexture("exitRight");
      else if (ship is PossessorShip)
        iconTex = AssetManager.GetTexture("share2");

      icon = new Sprite(Vector2.Zero, iconTex);
      icon.color = Color.AliceBlue;
      icon.Parent = this;

      back = new Sprite(Vector2.Zero, AssetManager.GetTexture("grey_button13_big"));
      back.color = Color.AliceBlue;
      back.Parent = this;

      Scale = 1.0f;
      Pos = pos;
    }
    public void Update()
    {
      Timer cd = ((IPlayerShip)ship).AbilityCooldown;
      if (!cd.Counting)
      {
        icon.alpha = 0.5f;
        back.alpha = 0.25f;
        value = 1.0;
      }
      else
      {
        icon.alpha = 0.2f;
        back.alpha = 0.125f;
        value = cd.Fraction;
      }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      back.LocalPos = Vector2.Zero;
      icon.LocalPos = Vector2.Zero;
      back.spriteRec = new Rectangle(0, 0, back.tex.Width, back.tex.Height);
      icon.spriteRec = new Rectangle(0, 0, icon.tex.Width, icon.tex.Height);
      back.Draw(spriteBatch);
      icon.Draw(spriteBatch);

      back.LocalPos = new Vector2(0, (int)(back.tex.Height * (1 - value) * back.Scale));
      icon.LocalPos = new Vector2(0, (int)(icon.tex.Height * (1 - value) * icon.Scale));
      back.spriteRec = new Rectangle(0, (int)(back.tex.Height * (1 - value)), back.tex.Width, (int)(back.tex.Height * value));
      icon.spriteRec = new Rectangle(0, (int)(icon.tex.Height * (1 - value)), icon.tex.Width, (int)(icon.tex.Height * value));
      back.Draw(spriteBatch);
      icon.Draw(spriteBatch);
      if (ship is BlinkerShip)
      {
        spriteBatch.DrawString(font, ((BlinkerShip)ship).AbilityCharges.ToString(), Pos + new Vector2(17, 12), Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.0f);
      }
    }
  }
}
