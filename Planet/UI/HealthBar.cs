using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class HealthBar
  {
    public float Value { get { return value; } set { this.value = value > maxValue ? maxValue : value; } }
    public float MaxValue { get { return maxValue; } protected set { value = maxValue; } }

    public Rectangle rec;
    Texture2D pixel;
    Texture2D frontTex, backTex;
    float value, maxValue;
    Color foreColor, backColor;
    bool mirrored;

    public HealthBar(Rectangle rec, float maxValue, float value, Color foreColor, Color backColor, bool mirrored = false)
    {
      pixel = AssetManager.GetTexture("pixel");
      frontTex = AssetManager.GetTexture("green_button05");
      backTex = AssetManager.GetTexture("grey_button05");
      this.rec = rec;
      this.maxValue = maxValue;
      this.value = value;
      this.foreColor = foreColor;
      this.backColor = backColor;
      this.mirrored = mirrored;
    }
    public void SetPos(Vector2 position)
    {
      rec.X = (int)position.X - (int)(rec.Width / 2.0f);
      rec.Y = (int)position.Y - (int)(rec.Height / 2.0f);
    }
    private Rectangle CalculateFrontRectangle()
    {
      int X, width;

      float fraction = value / maxValue;

      if (mirrored)
      {
        X = rec.X + (int)(rec.Width * (1 - fraction));
        width = rec.Width - (int)(rec.Width * (1 - fraction));
      }
      else
      {
        X = rec.X;
        width = (int)(rec.Width * fraction);
      }

      Rectangle front = new Rectangle(X, rec.Y, width, rec.Height);
      return front;
    }
    public void Draw(SpriteBatch sb)
    {
      sb.Draw(backTex, rec, null, backColor);
      sb.Draw(frontTex, CalculateFrontRectangle(), null, foreColor);
    }
  }
}
