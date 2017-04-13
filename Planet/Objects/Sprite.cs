﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Sprite : Transform
  {
    public bool Visible { get; set; }
    public Texture2D tex { get; private set; }
    public Vector2 origin;
    public Rectangle spriteRec;
    public Color color;
    public float alpha;
    public float layerDepth;
    public Sprite(Vector2 pos) : base(pos)
    {
      Visible = true;
      color = Color.White;
      alpha = 1f;
      layerDepth = 0f;
    }
    protected virtual void SetTexture(string tName, Rectangle? sourceRec = null)
    {
      try
      {
        this.tex = AssetManager.GetTexture(tName);
      }
      catch
      {
        throw;
      }
      if (sourceRec != null)
        spriteRec = (Rectangle)sourceRec;
      else
        spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
    }
    protected virtual void SetTexture(Texture2D tex, Rectangle? sourceRec = null)
    {
      if (tex == null)
        return;
      this.tex = tex;
      if (sourceRec != null)
        spriteRec = (Rectangle)sourceRec;
      else
        spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (Visible && tex != null)
        spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);
    }
  }
}
