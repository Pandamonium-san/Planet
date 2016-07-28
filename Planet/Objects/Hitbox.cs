using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Hitbox
  {
    private GameObject parent;
    private Rectangle hitbox;
    private Vector2 offset;
    public Rectangle Rectangle { get { return hitbox; } }

    public Hitbox(GameObject parent, int width, int height, Vector2 offset)
    {
      this.offset = offset;
      this.parent = parent;

      hitbox = new Rectangle(
          (int)(parent.Pos.X - parent.origin.X - (0.5f * ((width * (parent.Scale - 1.0f)) - offset.X)) / parent.Scale),
          (int)(parent.Pos.Y - parent.origin.Y - (0.5f * ((height * (parent.Scale - 1.0f)) - offset.Y)) / parent.Scale),
          (int)(width * parent.Scale - parent.hitboxOffset.X),
          (int)(height * parent.Scale - parent.hitboxOffset.Y)
          );
    }
    public void UpdatePosition()
    {
      hitbox.X = (int)(parent.Pos.X - parent.origin.X - (0.5f * ((hitbox.Width * (parent.Scale - 1.0f)) - offset.X)) / parent.Scale);
      hitbox.Y = (int)(parent.Pos.Y - parent.origin.Y - (0.5f * ((hitbox.Height * (parent.Scale - 1.0f)) - offset.Y)) / parent.Scale);
    }
    public bool Intersects(Hitbox other)
    {
      return hitbox.Intersects(other.hitbox);
    }
    public void SetOffset(Vector2 offset)
    {
      this.offset = offset;
    }
  }
}
