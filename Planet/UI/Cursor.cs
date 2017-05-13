using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Cursor : Sprite
  {
    public float Width
    {
      get { return width; }
      set { width = value; UpdateSubPos(); }
    }
    public float Height
    {
      get { return height; }
      set { height = value; UpdateSubPos(); }
    }

    float width, height;
    Sprite[] subSprites;

    public Cursor() : base(Vector2.Zero, AssetManager.GetTexture("cursor"))
    {
      subSprites = new Sprite[4];
      for (int i = 0; i < subSprites.Length; i++)
      {
        subSprites[i] = new Sprite(Pos, tex);
        subSprites[i].Rotation = i * (float)Math.PI / 2;
        subSprites[i].Parent = this;
      }
    }
    void UpdateSubPos()
    {
      for (int i = 0; i < subSprites.Length; i++)
      {
        if (i == 0)
          subSprites[i].LocalPos = new Vector2(-width, -height);
        else if (i == 1)
          subSprites[i].LocalPos = new Vector2(width, -height);
        else if (i == 2)
          subSprites[i].LocalPos = new Vector2(width, height);
        else if (i == 3)
          subSprites[i].LocalPos = new Vector2(-width, height);
      }
    }
    public override void Draw(SpriteBatch spriteBatch, float a = 1)
    {
      if (!Visible)
        return;
      for (int i = 0; i < subSprites.Length; i++)
      {
        subSprites[i].color = color;
        subSprites[i].alpha = alpha;
        subSprites[i].layerDepth = layerDepth;
        subSprites[i].Draw(spriteBatch, a);
      }
    }
  }
}
