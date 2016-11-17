using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Hitbox : Transform
  {
    private float localRadius;
    public float Radius;

    public Hitbox(GameObject parent, float radius)
      : base(parent.Pos, parent.Rotation, parent.Scale, parent)
    {
      localRadius = radius;
      Radius = radius * Scale;
    }
    public bool Collides(Hitbox other)
    {
      float radSum = Radius + other.Radius;
      float xDiff = Math.Abs(this.Pos.X - other.Pos.X);
      if (xDiff > radSum)
        return false;
      float yDiff = Math.Abs(this.Pos.Y - other.Pos.Y);
      if (yDiff > radSum)
        return false;
      float distance = xDiff * xDiff + yDiff * yDiff;
      if (distance <= (radSum * radSum))
        return true;
      return false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      Texture2D tex = AssetManager.GetTexture("Circle");
      Vector2 drawPos = Pos - Vector2.One * Radius;
      spriteBatch.Draw(tex, drawPos, null, Color.Blue * 0.5f, 0f, Vector2.Zero, (Radius / (tex.Width * 0.5f)), SpriteEffects.None, 0.1f);
    }

    protected override void Update()
    {
      base.Update();
      Radius = localRadius * Scale;
    }
  }

  //public class Hitbox
  //{
  //  private GameObject parent;
  //  private Rectangle hitbox;
  //  private Vector2 offset;
  //  public Rectangle Rectangle { get { return hitbox; } }

  //  public Hitbox(GameObject parent, int width, int height, Vector2 offset)
  //  {
  //    this.offset = offset;
  //    this.parent = parent;

  //    int boxWidth = (int)(width * parent.Scale - parent.hitboxOffset.X);
  //    int boxHeight = (int)(height * parent.Scale - parent.hitboxOffset.Y);

  //    hitbox = new Rectangle(
  //        (int)(parent.Pos.X - boxWidth / 2),
  //        (int)(parent.Pos.Y - boxHeight / 2),
  //        boxWidth,
  //        boxHeight
  //        );
  //  }
  //  public void UpdatePosition()
  //  {
  //    hitbox.X = (int)(parent.Pos.X - hitbox.Width / 2);
  //    hitbox.Y = (int)(parent.Pos.Y - hitbox.Height / 2);
  //    //hitbox.X = (int)(parent.Pos.X - parent.origin.X - (0.5f * ((hitbox.Width * (parent.Scale - 1.0f)) - offset.X)) / parent.Scale);
  //    //hitbox.Y = (int)(parent.Pos.Y - parent.origin.Y - (0.5f * ((hitbox.Height * (parent.Scale - 1.0f)) - offset.Y)) / parent.Scale);
  //  }
  //  public bool Collides(Hitbox other)
  //  {
  //    return hitbox.Intersects(other.hitbox);
  //  }
  //  public void SetOffset(Vector2 offset)
  //  {
  //    this.offset = offset;
  //  }
  //  public void Draw(SpriteBatch sb)
  //  {
  //    sb.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Blue * 0.5f);
  //  }
  //}

}
