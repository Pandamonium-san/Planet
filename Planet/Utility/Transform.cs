﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Transform
  {
    // properties
    public Vector2 Pos
    {
      get { return parent != null ? Utility.RotateVector2(parent.Pos + localPos, parent.Pos, parent.Rotation) : localPos; }
      set { localPos = Parent != null ? value - Parent.Pos : value; }
    }
    public float Rotation
    {
      get { return parent != null ? parent.Rotation + localRotation : localRotation; }
      set { localRotation = Parent != null ? value - Parent.Rotation : value; }
    }
    public float Scale
    {
      get { return parent != null ? parent.Scale * localScale : localScale; }
      set { localScale = parent != null ? value / parent.Scale : value; }
    }
    public Transform Parent
    {
      get { return parent; }
      set
      {
        Vector2 pos = Pos;
        parent = value;
        Pos = pos;
      }
    }

    // member variables
    private Transform parent;
    public Vector2 localPos;
    public float localRotation;
    public float localScale;

    public Transform(Vector2 pos, float rotation = 0.0f, float scale = 1.0f, Transform parent = null)
    {
      this.Parent = parent;
      this.Pos = pos;
      this.Rotation = rotation;
      this.Scale = scale;
    }
  }
}