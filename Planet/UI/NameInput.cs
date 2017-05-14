using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class NameInput : StylableText
  {
    public bool HideCursor { get; set; }
    public int Index { get; set; }
    public Cursor Cursor { get; set; }

    char[] name;

    public NameInput(SpriteFont font, Vector2 pos, float spacing, Color color, float scale = 1.0f)
      : base(font, "AAA", pos, spacing, color)
    {
      Scale = scale;
      Cursor = new Cursor();
      Cursor.color = color;
      Cursor.Width = font.MeasureString(array[Index].Get()).X / 2 * Scale;
      Cursor.Height = font.MeasureString(array[Index].Get()).Y / 2 * Scale;
      name = new char[3];
      for (int i = 0; i < name.Length; i++)
      {
        name[i] = 'A';
      }
      Cursor.Pos = array[Index].Pos;
    }
    public override string ToString()
    {
      string result = "";
      for (int i = 0; i < name.Length; i++)
      {
        result += name[i];
      }
      return result;
    }
    public void Up()
    {
      if (++name[Index] == 127)
        name[Index] = (char)32;
      array[Index].Set(name[Index].ToString());
    }
    public void Down()
    {
      if (--name[Index] == 31)
        name[Index] = (char)126;
      array[Index].Set(name[Index].ToString());
    }
    public void Left()
    {
      if (--Index == -1)
        Index = name.Length - 1;
      Cursor.Pos = array[Index].Pos;
    }
    public void Right()
    {
      if (++Index == name.Length)
        Index = 0;
      Cursor.Pos = array[Index].Pos;
    }
    public override void Draw(SpriteBatch sb, float a = 1)
    {
      base.Draw(sb, a);
      if (!HideCursor)
        Cursor.Draw(sb, a);
    }
  }
}
