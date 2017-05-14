using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class StylableText : Text
  {
    protected float spacing;
    protected Text[] array;

    public StylableText(SpriteFont font, string text, Vector2 pos, float spacing, Color color)
      : base(font, text, pos, color, Align.Center)
    {
      this.spacing = spacing;
      array = new Text[text.Length];
      for (int i = 0; i < array.Length; i++)
      {
        array[i] = new Text(font, text[i].ToString(), Vector2.Zero, color, Align.Center);
        array[i].Parent = this;
      }
      for (int i = 0; i < array.Length; i++)
      {
        array[i].LocalPos = new Vector2(i * spacing - GetWidth() / 2, 0);
      }
    }
    public float GetWidth()
    {
      //float w = Font.MeasureString(array.First().Get() + array.Last().Get()).X / 2;
      return (array.Length - 1) * spacing;
    }
    public Text Get(int index)
    {
      return array[index];
    }
    public Text[] GetArray()
    {
      return array;
    }
    public override void Draw(SpriteBatch sb, float a = 1)
    {
      if (!Visible)
        return;
      for (int i = 0; i < array.Length; i++)
      {
        array[i].Draw(sb, a);
      }
    }
  }
}
