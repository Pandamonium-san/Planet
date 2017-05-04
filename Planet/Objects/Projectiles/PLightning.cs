using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Planet
{
  class PLightning : Projectile
  {
    private int width;      // increases width on both sides so actual width is 2x bigger
    List<PHitScan> LBranches;

    public PLightning(World world, Vector2 start, Vector2 direction, float damage, int width, Ship instigator, float lifeTime = 0.4f)
      : base(world, AssetManager.GetTexture("pixel"), start, direction, 0, damage, instigator, lifeTime)
    {
      this.width = width;
      LBranches = new List<PHitScan>();
      GenerateLightning(Utility.RandomInt(0, 2) == 1);
    }
    protected void GenerateLightning(bool up)
    {
      string axiom = "F";
      //expand
      axiom = ExpandLTree(axiom, 3, up);
      axiom = axiom.Remove(0, 1);

      LinkedList<Vector2> positions = new LinkedList<Vector2>();
      LinkedList<Vector2> directions = new LinkedList<Vector2>();
      positions.AddFirst(Pos);
      directions.AddFirst(dir);

      for (int i = 0; i < axiom.Length; i++)
      {
        char c = axiom[i];
        Vector2 cPos = positions.First();
        Vector2 cDir = directions.First();
        float radians = Utility.RandomFloat((float)Math.PI / 12, (float)Math.PI / 4);
        float length = 30;//Utility.RandomFloat(20, 30);
        switch (c)
        {
          case 'G':
          case 'F':
            PHitScan branch = new PHitScan(world, tex, cPos, cDir, damage, true, width, length, instigator, initialLifeTime);
            branch.color = new Color(100, 100, 255);
            LBranches.Add(branch);
            positions.RemoveFirst();
            positions.AddFirst(cPos + cDir * (length - 1));
            break;
          case '+':
            directions.RemoveFirst();
            directions.AddFirst(Utility.RotateVector2(cDir, Vector2.Zero, radians));
            break;
          case '-':
            directions.RemoveFirst();
            directions.AddFirst(Utility.RotateVector2(cDir, Vector2.Zero, -radians));
            break;
          case '[':
            positions.AddFirst(cPos);
            directions.AddFirst(cDir);
            break;
          case ']':
            positions.RemoveFirst();
            directions.RemoveFirst();
            break;
        }
      }

      // Stores data about lightning shapes      
      //#if DEBUG
      //      string debugInfo = "";
      //      float totalAngle = 0;
      //      float totalArea;
      //      Vector2 topLeft = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
      //      Vector2 bottomRight = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

      //      foreach (PHitScan b in LBranches)
      //      {
      //        totalAngle += Utility.AngleBetweenVectors(b.dir, dir);

      //        //find smallest rectangle that contains lightning
      //        Action<Vector2> extremes = (v) =>
      //        {
      //          bottomRight.X = Math.Max(v.X, bottomRight.X);
      //          bottomRight.Y = Math.Max(v.Y, bottomRight.Y);
      //          topLeft.X = Math.Min(v.X, topLeft.X);
      //          topLeft.Y = Math.Min(v.Y, topLeft.Y);
      //        };
      //        extremes.Invoke(b.Pos);
      //        extremes.Invoke(b.Pos + b.dir * b.length);
      //      }

      //      Vector2 vArea = (bottomRight - topLeft);
      //      totalArea = vArea.X * vArea.Y;

      //      debugInfo += totalAngle.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "\t" + totalArea.ToString(System.Globalization.NumberFormatInfo.InvariantInfo); 
      //      debugInfo += "\n";

      //      DebugFunc.WriteToLog(debugInfo);
      //      //Debug.Write(totalArea);
      //#endif

    }

    protected string ExpandLTree(string axiom, int generations, bool up)
    {
      string result = axiom;
      for (int n = 0; n < generations; n++)
      {
        string temp = result;
        result = "";
        for (int i = 0; i < temp.Length; i++)
        {
          char c = temp[i];
          switch (c)
          {
            case 'F':
              if (up)
              {
                if (Utility.RandomFloat(0, 1) < 0.5)
                  result += "F[+-F]+F-F";
                else
                  result += "F+F-F";
              }
              else
              {
                if (Utility.RandomFloat(0, 1) < 0.5)
                  result += "F[-+F]-F+F";
                else
                  result += "F-F+F";
              }
              break;
            default:
              result += c;
              break;
          }
        }
      }
      return result;
    }
    protected override void DoUpdate(GameTime gt)
    {
      lifeTimer.Update(gt);
      for (int i = 0; i < LBranches.Count(); i++)
      {
        LBranches[i].Update(gt);
        LBranches[i].alpha = (1.0f - (float)lifeTimer.Fraction) * (1.2f - (float)i / (float)LBranches.Count);
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (IsActive)
      {
        for (int i = 0; i < LBranches.Count(); i++)
        {
          LBranches[i].Draw(spriteBatch);
        }
      }
    }
  }
}
