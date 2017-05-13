using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class SelectionBox : Sprite
  {
    public string Name { get; set; }
    protected Text text;
    public SelectionBox(Texture2D texture, Vector2 pos, string name) : base(pos, texture)
    {
      this.Name = name;
    }
    public Text GetText()
    {
      return text;
    }
    public void SetText(SpriteFont font, string text)
    {
      this.text = new Text(font, text, Pos, color);
      this.text.Parent = this;
      this.text.LocalPos = Vector2.Zero;
      this.text.LocalScale = 1.0f;
      this.text.LocalRotation = 0.0f;
      this.text.layerDepth = layerDepth + 0.001f;
    }
    public void SetText(Text text)
    {
      this.text = text;
    }
    public override void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      base.Draw(spriteBatch, a);
      if (text != null)
        text.Draw(spriteBatch, a);
    }
  }
}
