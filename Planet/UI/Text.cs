using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Text : Transform
  {
    public Vector2 origin;
    public SpriteFont font;
    public string text;
    public Color color;
    public float layerDepth;

    public Text(SpriteFont font, string text, Vector2 pos, Color color)
      : base(pos)
    {
      this.color = color;
      this.font = font;
      this.text = text;
      origin = font.MeasureString(text) / 2f;
      layerDepth = 0.0f;
    }
    public void Draw(SpriteBatch sb)
    {
      sb.DrawString(font, text, Pos, color, Rotation, origin, Scale, SpriteEffects.None, layerDepth);
    }
  }
}
