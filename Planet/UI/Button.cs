using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Button : Sprite
  {
    public string Name { get; set; }
    protected Text text;
    public Button(Vector2 pos, string name) : base(pos, AssetManager.GetTexture("blue_button04"))
    {
      this.Name = name;
    }
    public void AddText(SpriteFont font, string text)
    {
      this.text = new Text(font, text, Pos, color);
      this.text.Parent = this;
      this.text.layerDepth = layerDepth + 0.01f;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      if (text != null)
        text.Draw(spriteBatch);
    }
  }
}
