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
        parent = value;
        Pos = pos;
        if(parent != null)
          parent.AppendChild(this);
      }
    }

    // member variables
    private Transform parent;
    private List<Transform> children;
    public Vector2 localPos;
    public float localRotation;
    public float localScale;
    private Vector2 worldPos;
    private float worldRotation;
    private float worldScale;

    public Transform(Vector2 pos, float rotation = 0.0f, float scale = 1.0f, Transform parent = null)
    {
      this.Parent = parent;
      this.Pos = pos;
      this.Rotation = rotation;
      this.Scale = scale;
    }

    private void AppendChild(Transform child)
    {
      if(children == null)
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
    protected virtual void Update()
    {
      worldPos = parent != null ? Utility.RotateVector2(parent.Pos + localPos, parent.Pos, parent.Rotation) : localPos;
      worldRotation = parent != null ? parent.Rotation + localRotation : localRotation;
      worldScale = parent != null ? parent.Scale * localScale : localScale;
    }
  }
}
