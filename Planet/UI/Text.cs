using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Text : Sprite
  {
    public enum Align
    {
      Left,
      Center,
      Right
    }
    public Align alignment { get; set; }
    public SpriteFont font;
    private string text;

    public Text(SpriteFont font, string text, Vector2 pos, Color color, Align align = Align.Center)
      : base(pos, (Texture2D)null)
    {
      this.color = color;
      this.font = font;
      this.alignment = align;
      Set(text);
      layerDepth = 0.0f;
    }
    public Align GetAlignment()
    {
      return alignment;
    }
    public void SetAlignment(Align align)
    {
      this.alignment = align;
      UpdateAlignment();
    }
    public string Get()
    {
      return text;
    }
    public void Set(string text)
    {
      this.text = text;
      UpdateAlignment();
    }
    private void UpdateAlignment()
    {
      switch (alignment)
      {
        case Align.Left:
          origin = Vector2.Zero;
          break;
        case Align.Center:
          origin = font.MeasureString(text) / 2f;
          break;
        case Align.Right:
          origin = new Vector2(font.MeasureString(text).X, 0);
          break;
      }
    }
    public override void Draw(SpriteBatch sb, float a = 1.0f)
    {
      sb.DrawString(font, text, Pos, color * alpha * a, Rotation, origin, Scale, SpriteEffects.None, layerDepth);
    }
  }
}
