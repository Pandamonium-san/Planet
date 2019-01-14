using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class ShipInfo : Transform
  {
    Rectangle backPanel;
    Text shipName;
    Text weapons;
    Text ability;
    Text description;

    public ShipInfo(string name, Vector2 pos, Color color)
      : base(pos)
    {
      backPanel = new Rectangle();
      backPanel.Width = 700;
      backPanel.Height = 275;
      backPanel.X = (int)pos.X - backPanel.Width / 2;
      backPanel.Y = (int)pos.Y - backPanel.Height / 2 + 90;

      SpriteFont future18 = AssetManager.GetFont("future18");
      switch (name)
      {
        case "Rewinder":
          shipName = new Text(future18, "Rewinder", new Vector2(0, 0), color, Text.Align.Center);
          weapons = new Text(future18, "Gatling, Volcano", new Vector2(0, 50), color, Text.Align.Center);
          ability = new Text(future18, "Rewind", new Vector2(0, 100), color, Text.Align.Center);
          description = new Text(future18, "Turn back time to regain lost health\n         and escape a sticky situation", new Vector2(0, 150), color, Text.Align.Center);
          break;
        case "Blinker":
          shipName = new Text(future18, "Blinker", new Vector2(0, 0), color, Text.Align.Center);
          weapons = new Text(future18, "Burst, Wing", new Vector2(0, 50), color, Text.Align.Center);
          ability = new Text(future18, "Blink", new Vector2(0, 100), color, Text.Align.Center);
          description = new Text(future18, "Disappear into another plane momentarily\n              and reappear somewhere else", new Vector2(0, 150), color, Text.Align.Center);
          break;
        case "Possessor":
          shipName = new Text(future18, "Possessor", new Vector2(0, 0), color, Text.Align.Center);
          weapons = new Text(future18, "Laser, Grenade", new Vector2(0, 50), color, Text.Align.Center);
          ability = new Text(future18, "Possess", new Vector2(0, 100), color, Text.Align.Center);
          description = new Text(future18, "Latch on to enemies and defeat them\n         to take control of their ship", new Vector2(0, 150), color, Text.Align.Center);
          break;
      }
      shipName.Parent = this;
      weapons.Parent = this;
      ability.Parent = this;
      description.Parent = this;
      shipName.LocalPos = new Vector2(0, 0);
      weapons.LocalPos = new Vector2(0, 50);
      ability.LocalPos = new Vector2(0, 100);
      description.LocalPos = new Vector2(0, 160);
      shipName.Scale = 1.2f;
      description.Scale = 0.8f;
    }
    public void Draw(SpriteBatch spriteBatch, float alpha = 1.0f)
    {
      spriteBatch.Draw(AssetManager.Pixel, null, backPanel, null, null, 0.0f, null, new Color(0, 0, 0) * 0.2f * alpha, SpriteEffects.None, 0);
      shipName.Draw(spriteBatch, alpha);
      weapons.Draw(spriteBatch, alpha);
      ability.Draw(spriteBatch, alpha);
      description.Draw(spriteBatch, alpha);
    }
  }
}
