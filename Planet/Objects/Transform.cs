using Microsoft.Xna.Framework;
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
      get { return worldPos; }
      set
      {
        localPos = parent != null ? value - parent.Pos : value;
        worldPos = parent != null ? Utility.RotateVector2(parent.Pos + localPos, parent.Pos, parent.Rotation) : localPos;
        UpdateChildren();
      }
    }
    public float Rotation
    {
      get { return worldRotation; }
      set
      {
        localRotation = parent != null ? value - parent.Rotation : value;
        Update();
        UpdateChildren();
      }
    }
    public float Scale
    {
      get { return worldScale; }
      set
      {
        localScale = parent != null ? value / parent.Scale : value;
        worldScale = parent != null ? parent.Scale * localScale : localScale;
        UpdateChildren();
      }
    }
    public Transform Parent
    {
      get { return parent; }
      set
      {
        Vector2 pos = Pos;
        float scale = Scale;
        float rotation = Rotation;
        parent = value;
        Pos = pos;
        Scale = scale;
        Rotation = rotation;
        if (parent != null)
          parent.AppendChild(this);
      }
    }
    public Vector2 LocalPos { get { return localPos; } set { localPos = value; Update(); } }
    public float LocalRotation { get { return localRotation; } set { localRotation = value;  Update(); } }
    public float LocalScale { get { return localScale; } set { localScale = value;  Update(); } }

    // member variables
    private Transform parent;
    private List<Transform> children;
    private Vector2 localPos;
    private float localRotation;
    private float localScale;
    public Vector2 localOrigin;
    private Vector2 worldPos;
    private float worldRotation;
    private float worldScale;

    public Transform(Vector2 pos, float rotation = 0.0f, float scale = 1.0f, Transform parent = null)
    {
      this.Parent = parent;
      this.Pos = pos;
      this.Rotation = rotation;
      this.Scale = scale;
      localOrigin = Vector2.Zero;
    }

    private void AppendChild(Transform child)
    {
      if (children == null)
        children = new List<Transform>();
      children.Add(child);
    }
    private void UpdateChildren()
    {
      if (children == null)
        return;
      for (int i = 0; i < children.Count(); ++i)
      {
        if (children[i] == null)
          children.RemoveAt(i--);
        else
          children[i].Update();
      }
    }
    private void Update()
    {
      worldPos = parent != null ? Utility.RotateVector2(parent.Pos + localPos, parent.Pos + localOrigin, parent.Rotation) : localPos;
      worldRotation = parent != null ? parent.Rotation + localRotation : localRotation;
      worldScale = parent != null ? parent.Scale * localScale : localScale;
    }
  }
}
