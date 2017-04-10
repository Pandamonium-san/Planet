using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  //test
  class Laser : GameObject
  {
    List<LaserPart> parts = new List<LaserPart>();
    int nrOfParts;

    public Laser(Vector2 startPos, Vector2 endPos, World world)
        : base(startPos, world)
    {
      SetTexture("Laser");
      spriteRec = new Rectangle(0, 108, 51, 25);
      origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);

      nrOfParts = (int)Utility.Distance(startPos, endPos) / 25 + 1;
      for (int i = 0; i < nrOfParts; i++)
      {
        LaserPart part = new LaserPart(Vector2.Zero, this);
        part.localPos = new Vector2(0, (i + 1) * -25);
        parts.Add(part);
      }
    }

    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, Pos, spriteRec, Color.White, Rotation, origin, Scale, SpriteEffects.None, 0);
      foreach (LaserPart part in parts)
      {
        part.Draw(sb);
      }
    }

    private class LaserPart : Transform
    {
      Texture2D tex;
      Rectangle spriteRec;
      Vector2 origin;

      public LaserPart(Vector2 pos, Transform parent)
          : base(pos, 0, 1, parent)
      {
        tex = AssetManager.GetTexture("Laser");
        spriteRec = new Rectangle(0, 88, 51, 25);
        origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
      }

      public void Draw(SpriteBatch sb)
      {
        sb.Draw(tex, Pos, spriteRec, Color.White, Rotation, origin, Scale, SpriteEffects.None, 0);
      }
    }
  }
}
