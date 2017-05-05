using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class ValueBar : Transform
  {
    public float Value { get { return value; } set { this.value = value > maxValue ? maxValue : value; } }
    public float MaxValue { get { return maxValue; } protected set { value = maxValue; } }
    public int Width { get; set; }
    public int Height { get; set; }

    Texture2D frontTex, backTex;
    float value, maxValue;
    Color foreColor, backColor;
    bool mirrored;

    public ValueBar(Vector2 pos, int width, int height, float maxValue, Color foreColor, Color backColor, bool mirrored = false)
      : base(pos)
    {
      Width = width;
      Height = height;
      frontTex = AssetManager.GetTexture("pixel");
      backTex = frontTex;
      this.maxValue = maxValue;
      this.value = maxValue;
      this.foreColor = foreColor;
      this.backColor = backColor;
      this.mirrored = mirrored;
    }
    public ValueBar(Vector2 pos, int width, int height, float maxValue, Texture2D front, Texture2D back, bool mirrored = false)
      : base(pos)
    {
      Width = width;
      Height = height;
      frontTex = front;
      backTex = back;
      this.maxValue = maxValue;
      this.value = maxValue;
      this.foreColor = Color.White;
      this.backColor = Color.White;
      this.mirrored = mirrored;
    }
    private Rectangle BackRectangle()
    {
      int posX = (int)Pos.X;
      int posY = (int)Pos.Y;
      Rectangle front = new Rectangle(posX, posY, Width, Height);
      return front;
    }
    private Rectangle FrontRectangle()
    {
      int X, width;

      float fraction = value / maxValue;
      int posX = (int)Pos.X;
      int posY = (int)Pos.Y;

      if (mirrored)
      {
        X = posX + (int)(Width * (1 - fraction));
        width = Width - (int)(Width * (1 - fraction));
      }
      else
      {
        X = posX;
        width = (int)(Width * fraction);
      }

      Rectangle front = new Rectangle(X, posY, width, Height);
      return front;
    }
    public void Draw(SpriteBatch sb)
    {
      if (backTex != null)
        sb.Draw(backTex, BackRectangle(), null, backColor);
      sb.Draw(frontTex, FrontRectangle(), null, foreColor);
    }
  }
}
