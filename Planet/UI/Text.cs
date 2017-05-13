using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Text : Sprite
  {
    public enum Align
    {
      Left,
      Center,
      Right
    }
    public Align Alignment
    {
      get { return alignment; }
      set { alignment = value; UpdateAlignment(); }
    }
    public SpriteFont Font { get; set; }
    private Align alignment;
    private string text;

    public Text(SpriteFont font, string text, Vector2 pos, Color color, Align align = Align.Center)
      : base(pos, null)
    {
      this.Font = font;
      this.alignment = align;
      this.color = color;
      Set(text);
      layerDepth = 0.0f;
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
      switch (Alignment)
      {
        case Align.Left:
          origin = Vector2.Zero;
          break;
        case Align.Center:
          origin = Font.MeasureString(text) / 2f;
          break;
        case Align.Right:
          origin = new Vector2(Font.MeasureString(text).X, 0);
          break;
      }
    }
    public override void Draw(SpriteBatch sb, float a = 1.0f)
    {
      sb.DrawString(Font, text, Pos, color * alpha * a, Rotation, origin, Scale, SpriteEffects.None, layerDepth);
    }
  }
}
